using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackjackManager : MonoBehaviour
{
    public int cover;
    public List<int> Deck;
    public List<Card> playerCards;
    public List<Card> dealerCards;
    public int playerCount = 0;
    public int dealerCount = 0;
    public bool playerHaveAce = false;
    public bool dealerHaveAce = false;
    public Sprite[] images;

    public TextMesh playerPoint;
    public TextMesh dealerPoint;
    public TextMesh resultText;
    public TextMesh InstructionText;

    public bool instruction = true; //此值為True時允許玩家加牌(HIT)或停牌(STAND)

    // Start is called before the first frame update
    void Start()
    {

        //初始化牌庫
        for( int i = 0; i<52; i++)
        {
            Deck.Add(i);
        }

        int draw;
        cover = Random.Range(52, 54);

        //玩家發第一張牌
        draw = Deck[Random.Range(0, Deck.Count)];
        playerCards.Add(new Card(draw % 13 + 1, -7, 0, 0F, true, images[draw], images[cover], "PlayerCard0"));
        Deck.Remove(draw);
        //莊家發第一張牌
        draw = Deck[Random.Range(0, Deck.Count)];
        dealerCards.Add(new Card(draw % 13 + 1, 7, 0, 0F, true, images[draw], images[cover], "DealerCard0"));
        Deck.Remove(draw);

        //玩家發第二張牌
        draw = Deck[Random.Range(0, Deck.Count)];
        playerCards.Add(new Card(draw % 13 + 1, -6, 0, -0.1F, true, images[draw], images[cover], "PlayerCard1"));
        Deck.Remove(draw);
        //莊家發第二張牌
        draw = Deck[Random.Range(0, Deck.Count)];
        dealerCards.Add(new Card(draw % 13 + 1, 6, 0, 0.1F, false, images[draw], images[cover], "DealerCard1"));
        Deck.Remove(draw);

        CountPlayerPoint();
        CountDealerPoint();
    }

    //計算玩家牌面分數
    void CountPlayerPoint()
    {
        //玩家牌面計算 

        playerCount = 0;
        for (int i = 0; i < playerCards.Count; i++)
        {
            playerCount += playerCards[i].number;
            if (playerCards[i].number == 1)
            {
                playerHaveAce = true;
            }
        }

        //如果場上牌面有Ace，且算出得分小於等於11時，分數+10計算
        if (playerHaveAce && playerCount <= 11)
            playerCount += 10;

        //顯示於畫面
        //兩卡21點時，顯示BlackJack(天藍字)
        //超過21點時，顯示BURST(紅字)
        //五張卡過五關，顯示5 Cards(天藍字)，莊家沒有過五關
        if (playerCards.Count == 2 && playerCount == 21)
        {
            playerPoint.text = "BlackJack";
            playerPoint.color = Color.cyan;
            instruction = false;
            WinnerCheck();
        }
        else if (playerCount > 21)
        {
            playerPoint.text = "BURST";
            playerPoint.color = Color.red;
            instruction = false;
            WinnerCheck();
        }
        else if (playerCards.Count == 5)
        {
            playerPoint.text = "5 Cards";
            playerPoint.color = Color.cyan;
            instruction = false;
            WinnerCheck();
        }
        else
        {
            playerPoint.text = playerCount.ToString();
        }
    }

    //計算莊家牌面分數
    void CountDealerPoint()
    {
        //莊家BJ與否檢查
        if( dealerCards.Count==2 )
        {
            dealerCount = 0;
            for (int i = 0; i < dealerCards.Count; i++)
            {
                dealerCount += dealerCards[i].number;
                if (dealerCards[i].number == 1)
                {
                    dealerHaveAce = true;
                }
            }
            if(dealerCount==11 && dealerHaveAce)
            {
                dealerCards[1].CardOpen(6, 0, -0.1F);
            }
        }

        //莊家牌面
        dealerCount = 0;
        dealerHaveAce = false;
        for (int i = 0; i < dealerCards.Count; i++)
        {
            dealerCount += dealerCards[i].openCard ? (dealerCards[i].number) : 0;
            if (dealerCards[i].openCard && dealerCards[i].number == 1)
            {
                dealerHaveAce = true;
            }
        }


        if (dealerHaveAce && dealerCount <= 11)
            dealerCount += 10;

        //顯示於畫面
        //兩卡21點時，顯示BlackJack(天藍字)
        //超過21點時，顯示BURST(紅字)
        if (dealerCards.Count == 2 && dealerCount == 21)
        {
            dealerPoint.text = "BlackJack";
            dealerPoint.color = Color.cyan;
            instruction = false;
            WinnerCheck();
        }
        else if (dealerCount > 21)
        {
            dealerPoint.text = "BURST";
            dealerPoint.color = Color.red;
        }
        else
        {
            dealerPoint.text = dealerCount.ToString();
        }

    }

    void PlayerHit()
    {
        //抓取玩家手牌數量
        int cardCount = playerCards.Count;
        int draw;

        draw = Deck[Random.Range(0, Deck.Count)];
        playerCards.Add(new Card(draw % 13 + 1, -7F+cardCount, 0, -0.1F* cardCount, true, images[draw], images[cover], "PlayerCard"+cardCount.ToString() ));
        Deck.Remove(draw);

        CountPlayerPoint();
    }

    void DealerHit()
    {
        //抓取莊家手牌數量
        int cardCount = dealerCards.Count;
        int draw;

        draw = Deck[Random.Range(0, Deck.Count)];
        dealerCards.Add(new Card(draw % 13 + 1, 7 - cardCount, 0, -0.1F * cardCount, true, images[draw], images[cover], "DealerCard" + cardCount.ToString()));
        Deck.Remove(draw);

        CountDealerPoint();
    }

    //勝負判定
    void WinnerCheck()
    {
        //玩家先進行補牌，如果玩家先爆算輸
        //玩家得到BJ算贏，莊家和玩家都BJ算和。莊家會先檢查自己是否BJ，如果莊家BJ，玩家不補牌便輸掉
        //玩家過五關算贏
        //玩家沒爆，莊家爆算玩家贏
        //兩家都沒爆，那麼比較點數。較高者贏，同點平手

        if( !instruction )
        {
            if (playerCount > 21)
            {
                resultText.text = "LOSE";
                resultText.color = Color.gray;
            }
            else if (playerCount == 21 && playerCards.Count == 2 && dealerCount == 21 && dealerCards.Count == 2)
            {
                resultText.text = "PUSH";
                resultText.color = Color.cyan;
            }
            else if (dealerCount == 21 && dealerCards.Count == 2)
            {
                resultText.text = "LOSE";
                resultText.color = Color.gray;
            }
            else if (playerCount == 21 && playerCards.Count == 2)
            {
                resultText.text = "WIN";
                resultText.color = Color.yellow;
            }
            else if (playerCards.Count == 5)
            {
                resultText.text = "WIN";
                resultText.color = Color.yellow;
            }
            else if (dealerCount > 21)
            {
                resultText.text = "WIN";
                resultText.color = Color.yellow;
            }
            else
            {
                if (playerCount > dealerCount)
                {
                    resultText.text = "WIN";
                    resultText.color = Color.yellow;
                }
                else if (playerCount < dealerCount)
                {
                    resultText.text = "LOSE";
                    resultText.color = Color.gray;
                }
                else
                {
                    resultText.text = "PUSH";
                    resultText.color = Color.cyan;
                }
            }

            InstructionText.text = "Press R to restart";
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            else if(!instruction && Input.GetKeyDown(KeyCode.R))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("BlackjackMain");
            }
            else if(instruction && Input.GetKeyDown(KeyCode.DownArrow))
            {
                instruction = false;
                dealerCards[1].CardOpen(6, 0, -0.1F);
                CountDealerPoint();
                StartCoroutine(DealerDelayHit());
            }
            else if (instruction && Input.GetKeyDown(KeyCode.RightArrow))
            {
                PlayerHit();
                //玩家21點時自動停牌
                if(playerCount==21 && playerCards.Count<5)
                {
                    instruction = false;
                    dealerCards[1].CardOpen(6, 0, -0.1F);
                    CountDealerPoint();
                    StartCoroutine(DealerDelayHit());
                }
            }
            else if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    IEnumerator DealerDelayHit()
    {
        while (dealerCount < 17)
        {
            yield return new WaitForSeconds(0.5F);
            DealerHit();
        }
        WinnerCheck();
    }
}
