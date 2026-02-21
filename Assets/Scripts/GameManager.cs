using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;
    
    private long sweetPoints = 0;
    private int pointsPerClick = 1;
    private float pointsPerSecond = 0f;
    private float accumulatedPoints = 0f;

    public long SweetPoints => sweetPoints;
    public int PointsPerClick => pointsPerClick;
    public float PointsPerSecond => pointsPerSecond;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // scoreText가 연결되지 않은 경우 자동으로 찾기
        if (scoreText == null)
        {
            GameObject scoreObj = GameObject.Find("ScoreText");
            if (scoreObj != null)
            {
                scoreText = scoreObj.GetComponent<TMP_Text>();
            }
        }
        RefreshUI();
    }

    private void Update()
    {
        if (pointsPerSecond > 0f)
        {
            accumulatedPoints += pointsPerSecond * Time.deltaTime;
            if (accumulatedPoints >= 1f)
            {
                int whole = Mathf.FloorToInt(accumulatedPoints);
                sweetPoints += whole;
                accumulatedPoints -= whole;
                RefreshUI();
            }
        }
    }

    /// <summary>
    /// 클릭 또는 외부에서 점수를 추가합니다.
    /// </summary>
    public void AddPoints(int amount)
    {
        sweetPoints += amount;
        RefreshUI();
    }

    /// <summary>
    /// 업그레이드 구매 등에서 점수를 소비합니다.
    /// </summary>
    public bool SpendPoints(long cost)
    {
        if (sweetPoints < cost) return false;
        sweetPoints -= cost;
        RefreshUI();
        return true;
    }

    /// <summary>
    /// 클릭당 점수를 추가합니다.
    /// </summary>
    public void AddClickPower(int amount)
    {
        pointsPerClick += amount;
    }

    /// <summary>
    /// 초당 자동 획득 점수를 추가합니다.
    /// </summary>
    public void AddPerSecond(float amount)
    {
        pointsPerSecond += amount;
    }

    private void RefreshUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{sweetPoints:N0} Sweet Points";
        }
    }
}
