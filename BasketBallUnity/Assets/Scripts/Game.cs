using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Game : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    List<AudioClip> clipList;

    [SerializeField]
    List<Sprite> hoopSpriteList;

    [SerializeField]
    Text lblLevel, lblScore, lblBest, lblThrowm, lblMiss, lblAccuracy, lblHoopz, lblBestHitRate, lblBestInHistory;
    [SerializeField]
    Image imgPlayer, fgTimeBar;
    [SerializeField]
    GameObject playerShoot, hoopPref;
    [SerializeField]
    Transform hoopRoot, anchorTrans, ballTrans;
    [SerializeField]
    Button btnStart;
    [SerializeField]
    GameObject bgResult, win, lose;
    [SerializeField]
    Button btnVolume;
    [SerializeField]
    GameObject helpLayer;
    [SerializeField]
    List<Text> txtList;
    [SerializeField]
    GameObject a1, a2, a3;
    [SerializeField]
    Transform line;


    List<float> hoopYList = new List<float>() { 290f, 158.5f, 25 };

    List<float> firstHoopScore = new List<float>() { 500, 300, 200, 150, 100 };
    List<float> secondHoopScore = new List<float>() { 200, 150, 100, 50 };
    List<float> thirdHoopScore = new List<float>() { 150, 100, 50 };

    private List<Transform> firstHoopTransList = new List<Transform>();
    private List<Transform> secondHoopTransList = new List<Transform>();
    private List<Transform> thirdHoopTransList = new List<Transform>();

    int distance = 260;

    int loopTime_1 = 8;
    int loopTime_2 = 9;
    int loopTime_3 = 10;

    float distance_1, distance_2, distance_3;

    private int level = 1;
    private const int totalTime = 180;
    private int time = 180;
    private int needScore = 0; //level * 500
    private int curScore = 0; //当前分数
    private int bestShotPts = 0;//一次获得的最大分数
    private int shotMissed = 0;//失误次数
    private int shotTimes = 0;//射球次数
    private int shotHoopz = 0;//命中球数

    public static bool isStart = false;

    public static bool isLoad = false;

    public static bool volumeOpen = true;

    Vector3 ballSourcePos;

    float BestHitRate;
    int BestInHistory;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        btnVolume.transform.Find("spDisable").gameObject.SetActive(!volumeOpen);

        audioSource.volume = volumeOpen ? 1 : 0;

        BestHitRate = PlayerPrefs.GetFloat("BestHitRate", 0);
        BestInHistory = PlayerPrefs.GetInt("BestInHistory", 0);

        if (Mathf.Floor(BestHitRate) == Mathf.Ceil(BestHitRate))
            lblBestHitRate.text = string.Format("Best hit rate:{0}%", Mathf.Floor(BestHitRate));
        else
            lblBestHitRate.text = string.Format("Best hit rate:{0}%", BestHitRate.ToString("F1"));
        lblBestInHistory.text = string.Format("Best in history:{0}pts", BestInHistory);

        ballSourcePos = ballTrans.localPosition;
    }

    private void Start()
    {
        btnStart.gameObject.SetActive(true);
        //generateHoop();
    }

    void readyStart()
    {
        clcDistance(loopTime_1, loopTime_2, loopTime_3);
    }

    void clcDistance(int time1, int time2, int time3)
    {
        distance_1 = 1260 / 60.0f / time1;
        distance_2 = 1260 / 60.0f / time2;
        distance_3 = 1260 / 60.0f / time3;
    }

    void generateHoop()
    {

        int highScoreHoop = -1;
        int highScorePos = -1; 
        if(Random.Range(0,101) < 15)
        {
            highScoreHoop = Random.Range(1, 4);
            if(highScoreHoop == 1)
                highScorePos = Random.Range(0, 10);
            else if(highScoreHoop == 2)
                highScorePos = Random.Range(0, 8);
            else
                highScorePos = Random.Range(0, 9);
        }
        for(int i = 0; i < 10; i++)
        {
            Transform hoopTrans;
            if (i < firstHoopTransList.Count)
            {
                hoopTrans = firstHoopTransList[i];
            }
            else
            {
                GameObject hoopGo = GameObject.Instantiate(hoopPref);
                hoopGo.gameObject.SetActive(true);
                hoopGo.name = "First Hoop" + i;
                hoopTrans = hoopGo.transform;
                hoopTrans.SetParent(hoopRoot);
                hoopTrans.localScale = Vector3.one;
                hoopTrans.localRotation = Quaternion.identity;

                firstHoopTransList.Add(hoopTrans);
            }
            
            hoopTrans.localPosition = new Vector3(-575 + i * 260, hoopYList[0], 0);
            hoopTrans.GetComponent<Image>().sprite = hoopSpriteList[i % 5];
            Text lblPoint = hoopTrans.Find("lblPoint").GetComponent<Text>();
            lblPoint.gameObject.SetActive(true);
            if (highScoreHoop == 1 && i == highScorePos)
                lblPoint.text = "2000";
            else
                lblPoint.text = firstHoopScore[i % 5].ToString();

            
        }
        //第二行
        int timeAddSpecial = Random.Range(0, 10);
        int speedLowSpecial = (timeAddSpecial + 5) % 10;
        int index = 0;
        for(int i = 0; i < 10; i++)
        {
            Transform hoopTrans;
            if (i < secondHoopTransList.Count)
            {
                hoopTrans = secondHoopTransList[i];
                hoopTrans.Find("spAddTime").gameObject.SetActive(false);
                hoopTrans.Find("spLowSpeed").gameObject.SetActive(false);
                hoopTrans.Find("lblPoint").gameObject.SetActive(false);
            }
            else
            {
                GameObject hoopGo = GameObject.Instantiate(hoopPref);
                hoopGo.gameObject.SetActive(true);
                hoopGo.name = "First Hoop" + i;
                hoopTrans = hoopGo.transform;
                hoopTrans.SetParent(hoopRoot);
                hoopTrans.localScale = Vector3.one;
                hoopTrans.localRotation = Quaternion.identity;

                secondHoopTransList.Add(hoopTrans);
            }
            hoopTrans.localPosition = new Vector3(-450 + i * 260, hoopYList[1], 0);
            hoopTrans.GetComponent<Image>().sprite = hoopSpriteList[(i+2) % 5];
            if(i== timeAddSpecial)
            {
                hoopTrans.Find("spAddTime").gameObject.SetActive(true);
            }
            else if(i == speedLowSpecial)
            {
                hoopTrans.Find("spLowSpeed").gameObject.SetActive(true);
            }
            else
            {
                Text lblPoint = hoopTrans.Find("lblPoint").GetComponent<Text>();
                lblPoint.gameObject.SetActive(true);
                if (highScoreHoop == 2 && index == highScorePos)
                    lblPoint.text = "2000";
                else
                    lblPoint.text = secondHoopScore[index % 4].ToString();

                index++;
            }

            
        }
        //第三行
        for (int i = 0; i < 10; i++)
        {
            Transform hoopTrans;
            if (i < thirdHoopTransList.Count)
            {
                hoopTrans = thirdHoopTransList[i];
            }
            else
            {
                GameObject hoopGo = GameObject.Instantiate(hoopPref);
                hoopGo.gameObject.SetActive(true);
                hoopGo.name = "First Hoop" + i;
                hoopTrans = hoopGo.transform;
                hoopTrans.SetParent(hoopRoot);
                hoopTrans.localScale = Vector3.one;
                hoopTrans.localRotation = Quaternion.identity;

                thirdHoopTransList.Add(hoopTrans);
            }
            hoopTrans.localPosition = new Vector3(-575 + i * 260, hoopYList[2], 0);
            hoopTrans.GetComponent<Image>().sprite = hoopSpriteList[(i + 4) % 5];
            Text lblPoint = hoopTrans.Find("lblPoint").GetComponent<Text>();
            lblPoint.gameObject.SetActive(true);
            if (highScoreHoop == 1 && i == highScorePos)
                lblPoint.text = "2000";
            else
                lblPoint.text = thirdHoopScore[i % 3].ToString();

            
        }

        FirstHoopFirst = 0;
        FirstHoopLast = firstHoopTransList.Count - 1;

        SecondHoopFirst = 0;
        SecondHoopLast = secondHoopTransList.Count - 1;

        ThirdHoopFirst = 0;
        ThirdHoopLast = thirdHoopTransList.Count - 1;
    }

    void destroyHoop()
    {
        //for(int i = 0;i< firstHoopTransList.Count; i++)
        //{
        //    GameObject.Destroy(firstHoopTransList[i].gameObject);
        //}
        //for (int i = 0; i < secondHoopTransList.Count; i++)
        //{
        //    GameObject.Destroy(secondHoopTransList[i].gameObject);
        //}
        //for (int i = 0; i < thirdHoopTransList.Count; i++)
        //{
        //    GameObject.Destroy(thirdHoopTransList[i].gameObject);
        //}
        //firstHoopTransList.Clear();
        //secondHoopTransList.Clear();
        //thirdHoopTransList.Clear();
    }

    private int FirstHoopFirst;
    private int FirstHoopLast;

    private int SecondHoopFirst;
    private int SecondHoopLast;

    private int ThirdHoopFirst;
    private int ThirdHoopLast;

    private void LateUpdate()
    {
        if(isStart)
        {
            for(int i = 0;i< firstHoopTransList.Count; i++)
            {
                Vector3 curPos = firstHoopTransList[i].localPosition;
                firstHoopTransList[i].localPosition = new Vector3(curPos.x - distance_1, hoopYList[0], 0);
            }
            if(firstHoopTransList[FirstHoopFirst].localPosition.x < -1000)
            {
                firstHoopTransList[FirstHoopFirst].localPosition = new Vector3(firstHoopTransList[FirstHoopLast].localPosition.x + distance, hoopYList[0], 0);
                FirstHoopLast = FirstHoopFirst;
                FirstHoopFirst = (FirstHoopFirst + 1) % 10;                
            }

            for (int i = 0; i < secondHoopTransList.Count; i++)
            {
                Vector3 curPos = secondHoopTransList[i].localPosition;
                secondHoopTransList[i].localPosition = new Vector3(curPos.x - distance_2, hoopYList[1], 0);
            }
            if (secondHoopTransList[SecondHoopFirst].localPosition.x < -1000)
            {
                secondHoopTransList[SecondHoopFirst].localPosition = new Vector3(secondHoopTransList[SecondHoopLast].localPosition.x + distance, hoopYList[1], 0);
                SecondHoopLast = SecondHoopFirst;
                SecondHoopFirst = (SecondHoopFirst + 1) % 10;
            }

            for (int i = 0; i < thirdHoopTransList.Count; i++)
            {
                Vector3 curPos = thirdHoopTransList[i].localPosition;
                thirdHoopTransList[i].localPosition = new Vector3(curPos.x - distance_3, hoopYList[2], 0);
            }
            if (thirdHoopTransList[ThirdHoopFirst].localPosition.x < -1000)
            {
                thirdHoopTransList[ThirdHoopFirst].localPosition = new Vector3(thirdHoopTransList[ThirdHoopLast].localPosition.x + distance, hoopYList[2], 0);
                ThirdHoopLast = ThirdHoopFirst;
                ThirdHoopFirst = (ThirdHoopFirst + 1) % 10;
            }
        }
         
    }


    public void shoot()
    {
        shotTimes++;
        lblThrowm.text = string.Format("Balls thrown:{0}", shotTimes);
        score_1 = 0;
        score_2 = 0;
        score_3 = 0;

        anchorTrans.gameObject.SetActive(false);
        line.gameObject.SetActive(false);

        Vector3 targetPos = anchorTrans.transform.localPosition;

        int checkHoopIndex = -1;
        for(int i = 0;i < hoopYList.Count;i++)
        {
            if(targetPos.y >= hoopYList[i])
            {
                checkHoopIndex = i;
                break;
            }    
        }
        ballTrans.DOKill();
        ballTrans.localPosition = ballSourcePos;
        ballTrans.gameObject.SetActive(true);
        playerShoot.gameObject.SetActive(true);
        imgPlayer.enabled = false;
        ballTrans.DOScale(Vector3.one * 0.8f, 0.4f).SetDelay(0.1f);
        ballTrans.DOLocalMove(anchorTrans.transform.localPosition, 0.5f).OnComplete(() =>
        {
            playerShoot.gameObject.SetActive(false);
            imgPlayer.enabled = true;

            if (checkHoopIndex > -1)
                checkHoopEnter(checkHoopIndex);
            else
                ballFollowNext(checkHoopIndex);
        });
    }

    int score_1, score_2, score_3;


    void checkHoopEnter(int hoopIndex)
    {
        if(hoopIndex == -1)
        {
            result();
            return;
        }
        List<Transform> checkTransList;
        if (hoopIndex == 0)
            checkTransList = firstHoopTransList;
        else if (hoopIndex == 1)
            checkTransList = secondHoopTransList;
        else
            checkTransList = thirdHoopTransList;

        Vector3 ballPos = ballTrans.localPosition;
        float ballCenterX = ballPos.x;
        float ballBonusLeft = ballPos.x - 27;
        float ballBonusRight = ballPos.x + 27;
        float hoopCenterX = 0, hoopBonusLeft = 0, hoopBonusRight = 0;
        int hoopCheckIndex = -1;
        for(int i = 0;i < checkTransList.Count; i++)
        {
            hoopCenterX = checkTransList[i].localPosition.x;
            hoopBonusLeft = checkTransList[i].localPosition.x - 64.5f;
            hoopBonusRight = checkTransList[i].localPosition.x + 64.5f;
            if(ballBonusRight >= hoopBonusLeft && ballBonusLeft <= hoopBonusRight)
            {
                hoopCheckIndex = i;
                break;
            }
        }

        hoopIndex += 1;

        if (hoopCheckIndex != -1)
        {
            if(ballCenterX >= hoopCenterX - 30 *0.99f && ballCenterX <= hoopCenterX + 30 * 0.99f)//中了
            {
                audioSource.PlayOneShot(clipList[1]);
                ballTrans.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                if (hoopIndex == 1)
                {
                    score_1 = int.Parse(checkTransList[hoopCheckIndex].Find("lblPoint").GetComponent<Text>().text);
                    txtList[0].transform.SetParent(checkTransList[hoopCheckIndex]);
                    txtList[0].transform.localPosition = new(117, 25, 0);
                    txtList[0].text = score_1.ToString();
                    txtList[0].DOColor(new Color(1,1,1,0), 0.5f).SetDelay(0.2f).OnComplete(()=>{
                        txtList[0].text = "";
                        txtList[0].color = Color.white;
                    });
                }
                else if(hoopIndex == 2)
                {
                    if (checkTransList[hoopCheckIndex].Find("lblPoint").gameObject.activeSelf)
                    {
                        if(score_1 > 0)
                        {
                            score_2 = int.Parse(checkTransList[hoopCheckIndex].Find("lblPoint").GetComponent<Text>().text) * 2;
                            txtList[1].transform.SetParent(checkTransList[hoopCheckIndex]);
                            txtList[1].transform.localPosition = new(117, 25, 0);
                            txtList[1].text = string.Format("{0}X2", score_2 / 2);
                            txtList[1].DOColor(new Color(1, 1, 1, 0), 0.5f).SetDelay(0.2f).OnComplete(() => {
                                txtList[1].text = "";
                                txtList[1].color = Color.white;
                            });
                        }
                        else
                        {
                            score_2 = int.Parse(checkTransList[hoopCheckIndex].Find("lblPoint").GetComponent<Text>().text);
                            txtList[1].transform.SetParent(checkTransList[hoopCheckIndex]);
                            txtList[1].transform.localPosition = new(117, 25, 0);
                            txtList[1].text = score_2.ToString();
                            txtList[1].DOColor(new Color(1, 1, 1, 0), 0.5f).SetDelay(0.2f).OnComplete(() => {
                                txtList[1].text = "";
                                txtList[1].color = Color.white;
                            });
                        }
                    }
                    else
                    {
                        if (checkTransList[hoopCheckIndex].Find("spLowSpeed").gameObject.activeSelf)
                        {
                            clcDistance(loopTime_1 + 5, loopTime_2 + 5, loopTime_3 + 5 );
                        }
                        else
                        {
                            time += 10;
                        }                            
                    }
                }
                else
                {
                    if (score_1 > 0 && score_2 > 0)
                    {
                        score_3 = int.Parse(checkTransList[hoopCheckIndex].Find("lblPoint").GetComponent<Text>().text) * 3;
                        txtList[2].transform.SetParent(checkTransList[hoopCheckIndex]);
                        txtList[2].transform.localPosition = new(117, 25, 0);
                        txtList[2].text = string.Format("{0}X3", score_3 / 3);
                        txtList[2].DOColor(new Color(1, 1, 1, 0), 0.5f).SetDelay(0.2f).OnComplete(() => {
                            txtList[2].text = "";
                            txtList[2].color = Color.white;
                        });
                    }
                    else if(score_1 > 0 || score_2 > 0)
                    {
                        score_3 = int.Parse(checkTransList[hoopCheckIndex].Find("lblPoint").GetComponent<Text>().text) * 2;
                        txtList[2].transform.SetParent(checkTransList[hoopCheckIndex]);
                        txtList[2].transform.localPosition = new(117, 25, 0);
                        txtList[2].text = string.Format("{0}X2", score_3 / 2);
                        txtList[2].DOColor(new Color(1, 1, 1, 0), 0.5f).SetDelay(0.2f).OnComplete(() => {
                            txtList[2].text = "";
                            txtList[2].color = Color.white;
                        });
                    }
                    else
                    {
                        score_3 = int.Parse(checkTransList[hoopCheckIndex].Find("lblPoint").GetComponent<Text>().text);
                        txtList[2].transform.SetParent(checkTransList[hoopCheckIndex]);
                        txtList[2].transform.localPosition = new(117, 25, 0);
                        txtList[2].text = score_3.ToString();
                        txtList[2].DOColor(new Color(1, 1, 1, 0), 0.5f).SetDelay(0.2f).OnComplete(() => {
                            txtList[2].text = "";
                            txtList[2].color = Color.white;
                        });
                    }
                }
                ballFollowNext(hoopIndex);
            }
            else if(ballCenterX < hoopCenterX - 30 * 0.99f) //左边
            {
                ballTrans.DOKill();
                ballTrans.DOScale(Vector3.one * 0.8f, 0.35f).OnComplete(() =>
                {
                    ballFollowNext(hoopIndex);
                });
                ballTrans.DOLocalJump(new Vector3(ballPos.x - 80, ballPos.y + 40, 0), 30, 1, 0.4f);
            }
            else if(ballCenterX > hoopCenterX + 30 * 0.99f) //右边
            {
                ballTrans.DOKill();
                ballTrans.DOScale(Vector3.one * 0.8f, 0.35f).OnComplete(() =>
                {
                    ballFollowNext(hoopIndex);
                });
                ballTrans.DOLocalJump(new Vector3(ballPos.x + 40, ballPos.y + 40, 0), 60, 1, 0.4f);
            }
            else
            {
                ballFollowNext(hoopIndex);
            }
        }
        else
        {
            ballFollowNext(hoopIndex);
        }            
    }

    void ballFollowNext(int nextIndex)
    {
        ballTrans.DOKill();
        if (nextIndex >= 0 && nextIndex <= hoopYList.Count - 1)
        {
            ballTrans.DOScale(Vector3.one * 0.8f, 0.35f).OnComplete(() =>
            {
                ballTrans.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                checkHoopEnter(nextIndex);
            });
            ballTrans.DOLocalMoveY(hoopYList[nextIndex], 0.4f);
        }
        else
        {
            
            ballTrans.DOLocalMoveY(-233, 0.5f).OnComplete(()=>
            {
                ballTrans.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                ballTrans.gameObject.SetActive(false);
            });
            result();
        }
    }

    void result()
    {
        int thisScore = score_1 + score_2 + score_3;
        curScore = curScore + thisScore;
        
        if(thisScore > bestShotPts)
        {
            bestShotPts = thisScore;
            lblBest.text = string.Format("Best shot:{0}", bestShotPts);
        }
        if(thisScore == 0)
        {
            shotMissed++;
            lblMiss.text = string.Format("Shots missed:{0}", shotMissed);
        }
        else
        {
            shotHoopz++;
            lblHoopz.text = string.Format("Hoopz:{0}", shotHoopz);
        }
        float accuracy = (shotHoopz * 100 / (float)shotTimes);
        if(Mathf.Ceil(accuracy) == Mathf.Floor(accuracy) )
            lblAccuracy.text = string.Format("Accuracy:{0}%", shotTimes == 0 ? 0: Mathf.Floor(accuracy));
        else
            lblAccuracy.text = string.Format("Accuracy:{0}%", shotTimes == 0 ? 0 : accuracy.ToString("F1"));
        
        lblScore.text = curScore.ToString();
    }

    IEnumerator countdown()
    {
        a1.SetActive(true);
        yield return new WaitForSeconds(1);
        a1.SetActive(false);
        a2.SetActive(true);
        yield return new WaitForSeconds(1);
        a2.SetActive(false);
        a3.SetActive(true);
        yield return new WaitForSeconds(1);
        a3.SetActive(false);
        isStart = true;
        while (time > 0)
        {
            time--;

            fgTimeBar.fillAmount = (time / (float)totalTime);
            if(time <= 0)
            {
                isStart = false;
                destroyHoop();
                bgResult.gameObject.SetActive(true);

                if (curScore > BestInHistory)
                {
                    BestInHistory = curScore;
                    PlayerPrefs.SetInt("BestInHistory", BestInHistory);
                    lblBestInHistory.text = string.Format("Best in history:{0}pts", BestInHistory);
                }
                float accuracy = (shotHoopz * 100 / (float)shotTimes);
                if (accuracy > BestHitRate)
                {
                    BestHitRate = accuracy;
                    PlayerPrefs.SetFloat("BestHitRate", BestHitRate);
                    if (Mathf.Floor(BestHitRate) == Mathf.Ceil(BestHitRate))
                        lblBestHitRate.text = string.Format("Best hit rate:{0}%", Mathf.Floor(BestHitRate));
                    else
                        lblBestHitRate.text = string.Format("Best hit rate:{0}%", BestHitRate.ToString("F1"));

                }
                //win.gameObject.SetActive(true);
                //if (curScore >= needScore)
                //{
                //    win.gameObject.SetActive(true);
                //}
                //else
                //{
                //    lose.gameObject.SetActive(true);
                //}
            }
            else
                yield return new WaitForSeconds(1);
        }
    }

   public void onStart()
    {
        if(!isStart)
        {            
            btnStart.gameObject.SetActive(false);
            level = 1;
            curScore = 0;
            bestShotPts = 0;
            shotTimes = 0;
            shotMissed = 0;
            shotHoopz = 0;
            time = totalTime;
            needScore = level * 500;
            fgTimeBar.fillAmount = 1;
            lblLevel.text = string.Format("Level:{0}", level);
            lblScore.text = curScore.ToString();
            lblBest.text = string.Format("Best shot:{0}", bestShotPts);
            lblThrowm.text = string.Format("Balls thrown:{0}", shotTimes);
            lblMiss.text = string.Format("Shots missed:{0}", shotMissed);
            lblAccuracy.text = string.Format("Accuracy:{0}%", shotTimes == 0 ? 0 :(shotHoopz / (float)shotTimes * 100).ToString("F1"));
            lblHoopz.text = string.Format("Hoopz:{0}", shotHoopz);
            StartCoroutine(countdown());

            readyStart();
            generateHoop();            
        }
    }

    public void onRestart()
    {
        bgResult.gameObject.SetActive(false);
        win.gameObject.SetActive(false);
        lose.gameObject.SetActive(false);

        onStart();

        //if (curScore >= needScore)
        //{
        //    level++;
        //    time = 180;
        //    needScore = level * 500;
        //    fgTimeBar.fillAmount = 1;
        //    lblLevel.text = string.Format("Level:{0}", level);
        //    readyStart();
        //    generateHoop();
        //    StartCoroutine(countdown());
        //    isStart = true;
        //}
        //else
        //{
        //    onStart();
        //}
    }

    public void onBtnClick(string name )
    {
        audioSource.PlayOneShot(clipList[0]);
        if (name == "btnVolume")
        {
            volumeOpen = !volumeOpen;
            btnVolume.transform.Find("spDisable").gameObject.SetActive(!volumeOpen);

            audioSource.volume = volumeOpen ? 1 : 0;
        }
        else if(name == "btnHelp")
        {
            helpLayer.gameObject.SetActive(true);
        }
        else if(name == "btnHome")
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("LoginScene");
        }else if(name == "btnOk")
        {
            helpLayer.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        isStart = false;
    }
}
