using Common;
using SpaceShooter;
using System;
using System.Collections;
using UnityEngine;

namespace GentianoseRealDolls
{
    public enum ToiletStat
    {
        Poo,
        AnalSpray,
        Pee,
        Bath,
        BrushTeeth
    }
    /// <summary>
    /// Модель играбельного персонажа
    /// В отличие от других подобных игр
    /// здесь мы управляем не человеком,
    /// а маломорфным животным
    /// Главным героем является серый бамбуковый лемур
    /// Все подобные звери называются куклами
    /// Кукла - не потому что "без сердца"
    /// Кукла - потому что милый
    /// </summary>
    [RequireComponent(typeof(Destructible))]
    public class Doll : MonoBehaviour
    {
        private const long TicksInSecond = 10000000;
        private long StatsPeriod = 9;
        [SerializeField] private DollController m_Controller;
        public DollController DollController => m_Controller == null ? GetComponent<DollController>() : m_Controller;


        public const float sprayAmountInCare = 2.0f;
        private const string fileName = "doll.dat";


        public const float MaxBath = 34.0f;
        public const float MaxBrushTeeth = 33.0f;
        public const float MaxStat = 100.0f;

        public const float MaxLooStat = 11.0f;


        public const float StepLooStat = 0.11f;
        public const float StepBath = 0.34f;
        public const float StepBrushTeeth = 0.33f;
        public const float StepAnalGlandSecretions = 0.04f;

        // Вся струя, если 11 - это та, что нужна для прочистки
        public const float TotalAnalGlandVolumeQuotient = 2.618f;

        public const float FullVolumeAnalGlandUnified = TotalAnalGlandVolumeQuotient * MaxLooStat;

        private long TicksInMinute = 600000000;

        [Header("Meta of a Doll")]
        [SerializeField] private int m_DollID;
        public int DollID => m_DollID;

        [SerializeField] private string m_CharacterName;

        [SerializeField] private DollAsset m_Asset;
        public DollAsset Asset { get { return m_Asset; } }
        #region Data_Pet

        [Header("At home")]

        [Range(0.0f, 34.0f)]
        [SerializeField] private float m_Bath;

        [Range(0.0f, 33.0f)]
        [SerializeField] private float m_BrushTeeth;

        [Range(0.0f, 11.0f)]
        [SerializeField] private float m_LooSpray;
        public float AnalGlandHealth => m_LooSpray;

        [Range(0.0f, 11.0f)]
        [SerializeField] private float m_LooPee;

        [Range(0.0f, 11.0f)]
        [SerializeField] private float m_LooPoo;
        public float PooPoints => m_LooPoo;

        [Range(0.0f, 100.0f)]
        [SerializeField] private float m_FoodHunger;
        public float FoodHunger => m_FoodHunger;

        [Range(0.0f, 100.0f)]
        [SerializeField] private float m_Sleep;
        public float Sleep => m_Sleep;

        [Range(0.0f, 100.0f)]
        [SerializeField] private float m_Joy;
        public float Joy => m_Joy;

        public float Loo => m_LooPee + m_LooPoo + m_LooSpray;
        public float Bathroom => Loo + m_Bath + m_BrushTeeth;


        public bool FullSleep => m_Sleep >= MaxStat;

        #endregion

        #region Data_Combat

       

        private GRDTimer t;

       // private bool m_IsSleeping;
        public enum Attribute
        {
            Water,
            Wind,
            Fire,
            Earth
        }
        public enum ChemicalClass
        {
            Glycoside,
            Alkaloid,
            Polyphenol,
            Inorganic
        }

        [Header("In Dolls City")]

        [SerializeField] private float m_AnalGlandVolume;
        public float AnalGlandVolume => m_AnalGlandVolume;
        [Range(0, 18)]
        [SerializeField] private float m_AnalSprayAmount;
        public float AnalSprayAmount => m_AnalSprayAmount;

        [SerializeField] private Attribute m_Attribute;

        [SerializeField] private ChemicalClass m_ChemicalClass;

        [SerializeField] private string m_Chemical;

        [SerializeField] private int m_SprayDamage;  // AOE 


        #endregion

        #region Data_Media

        [Tooltip("0 - звук FX при обычной атаке, 1 - звук FX при заряженной атаке, 2 - возглас при обычной атаке" +
            "3 - звук FX при флемене, 4 - звук животного при флемене, 5 - реплика при флемене " +
            "6 - FX фуньки, 7 - звук животного при фуньке, 8 - реплика при фуньке" +
            "9 - звук мочеиспускания")]

        [SerializeField] private AudioSource[] m_Sounds;

        public AudioSource[] Sounds => m_Sounds;

        [SerializeField] private GameObject m_PoopPrefab;
        [SerializeField] private Turret m_Anus;
        public Turret AnusNipplesTurret => m_Anus;
        [SerializeField] private AIPointPatrol m_Wisp;
        public AIPointPatrol Wisp => m_Wisp;

        public bool PooStart { get; internal set; }

        [SerializeField] private Animator m_Animator;



        [Tooltip("quadrupedal and bipedal forms")]
        public Vector3[] centerOfMassVersions;



        #endregion

        [SerializeField] private SpaceShip petAsSpaceShip;
        public SpaceShip PetAsSpaceShip => petAsSpaceShip;
        private Rigidbody m_rb;
        public Rigidbody RB => m_rb;
        [SerializeField] private Transform m_CoM;

        public static long PreviousTime;

        public static long CurrentTime;

        private event Action OnDollJumps;

        // public bool Sleeping => m_IsSleeping;
        //TODO: в начало///
        //     private const string fileName = "dollSleep.dat";


        [Tooltip("0 - shit, 1 - spray, 2 - pee, 3 - bath, 4 - brushTeeth")]
        private float[] m_ToiletStats;
        public float[] ToiletStats => m_ToiletStats;


       [SerializeField]  private DollScaleValues m_ScaleValues;

        [SerializeField] private bool m_IsActiveDollInParty;
        public bool ActiveDollInPartyStatus => m_IsActiveDollInParty;
        public void SetActiveDoll(bool active)
        {
            m_IsActiveDollInParty = active;
        }
        public Animator EntityAnimator => m_Animator;
        



        public const int LocationsNumber = 2;
        #region Unity Event
        private void Awake()
        {
            print("Hello, I'm " + m_CharacterName);

            m_rb = GetComponent<Rigidbody>();

            m_Controller = GetComponent<DollController>();

        }

        private void OnDestroy()
        {
            print($"Goodbye, Interface 336n! Your {m_CharacterName}");


            SaveStats();
        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

         //   m_PoopManager.OnPoopDeposit += PooCareLevelFull();
            StatsPeriod = GRDPlayer.StatsPeriod;


            m_ScaleValues = AllDollCharacters.Instance.GetDollData(DollID);



            print(Loo);
            petAsSpaceShip = GetComponent<SpaceShip>();

            if (m_ScaleValues == null)
            {
                m_ScaleValues = new DollScaleValues();
                AllDollCharacters.Instance.AddDoll(m_ScaleValues);
            }

        }


        // Update is called once per frame
        void Update()
        {

           
            if (Input.GetKeyDown(KeyCode.C))
            {
                transform.up = Vector3.up;
                transform.position += new Vector3(0, 0.3f, 0);
            }
           


            m_LooSpray = MaxLooStat - MaxLooStat * (m_AnalSprayAmount - m_AnalGlandVolume * 0.6f)
                / (m_AnalGlandVolume - m_AnalGlandVolume * 0.6f);

            m_LooSpray = Mathf.Max(m_LooSpray, 0);
            m_LooSpray = Mathf.Min(m_LooSpray, MaxLooStat);

            //print(m_ScaleValues.Positions[0]);
        }

        #endregion

        #region Stats Storage
       
        IEnumerator PositionWrite()
        {
            yield return new WaitForSeconds(4.25f);
            AllDollCharacters.Instance.SetDollData(m_ScaleValues);
            StartCoroutine(PositionWrite());
        }



        /// <summary>
        /// Строка имеет вид SSXXXXXXXXYYYYYYYYZZZZZZZZ,
        /// где SS - сцена,
        /// XXXXXXXX - координата в метрах с десятыми (с запятой) и знаком +/-:
        /// например, -14300,5 - на запад на 14 км 300 м 50 см
        /// Аналогично с другими координатами
        /// </summary>
        /// <param name="address"></param>



        #endregion

        #region Service Data Doll





        #endregion


        #region Public API

        public void ChangeSleep(bool isSleeping)
        {
            if (isSleeping)
            {
                if (m_Sleep < MaxStat)
                    m_Sleep += 1;
            }
            else
            {
                if (m_Sleep > 0)
                    m_Sleep -= 1;
            }


            SaveStats();

        }

        public void SetSleep(float sleep)
        {
            m_Sleep = Mathf.Clamp(sleep, 0, MaxStat);

            SaveStats();


        }
        public void SetFoodHunger(float foodHunger)
        {
            m_FoodHunger = Mathf.Clamp(foodHunger, 0, MaxStat);

            SaveStats();
        }
       

        public void ReduceNonSleepStats()
        {

            if (m_AnalSprayAmount < m_AnalGlandVolume)
                m_AnalSprayAmount += StepAnalGlandSecretions * m_AnalGlandVolume;
            if (m_AnalSprayAmount > m_AnalGlandVolume)
                m_AnalSprayAmount = m_AnalGlandVolume;

            m_ScaleValues.AnalSprayAmount = m_AnalSprayAmount;

            m_LooSpray = MaxLooStat * (m_AnalSprayAmount - m_AnalGlandVolume * 0.6f) /
                m_AnalGlandVolume * 0.4f;

            m_LooSpray = Mathf.Min(m_LooSpray, MaxLooStat);

            if (m_LooPoo > 0)
                m_LooPoo -= StepLooStat;

            if (m_LooPee > 0)
                m_LooPee -= StepLooStat;

            if (m_Bath > 0)
                m_Bath -= StepBath;

            if (m_BrushTeeth > 0)
                m_BrushTeeth -= StepBrushTeeth;

            if (m_FoodHunger > 0)
                m_FoodHunger -= 1;

            m_ToiletStats = new float[5]
            {
                m_LooPoo,
                m_LooSpray,
                m_LooPee,
                m_Bath,
                m_BrushTeeth
            };

            SaveStats();

            print(PreviousTime);
        }



        //
      
        public void SaveStats()
        {
            m_ScaleValues.LooPoo = m_LooPoo;
            m_ScaleValues.AnalSprayAmount = m_AnalSprayAmount;
            m_ScaleValues.LooPee = m_LooPee;
            m_ScaleValues.Bath = m_Bath;
            m_ScaleValues.BrushTeeth = m_BrushTeeth;

            m_ScaleValues.FoodHunger = m_FoodHunger;
            m_ScaleValues.Sleep = m_Sleep;

            m_ScaleValues.dollID = DollID;

            

            AllDollCharacters.Instance.SetDollData(m_ScaleValues);
        }
        

       

        #endregion


        [Tooltip("0 - база, 1 - шаг, 2 - рысь/кентер, 3 - галоп, 4 - фунька, 5 - какание, 6 - поднять хвост")]
        [Range(0f, 6f)]
        int state = 0;
        public int State
        {
            get { return state; }
            set
            {
                if (value >= 0 &&  value <= 6) 
                    state = value;
            }
        }

        //[Range(1f, 3f)]
        // public int gaitState;
        #region Pet Care
        private Action PooCareLevelFull()
        {
            return () =>
            {
                m_LooPoo = 11f;
            };

        }

        public void SetMaxPooCare()
        {
            m_LooPoo = MaxLooStat;
            SaveStats();
        }

        #endregion

        #region CharacterSkills
        /// <summary>
        /// Распылить секрет
        /// Анальная железа освобождается на ~18%
        /// </summary>
        public void Spray()
        {
            if (m_AnalSprayAmount > m_AnalGlandVolume / 5)
            {

                m_AnalSprayAmount -= m_AnalGlandVolume / 5;



                m_LooSpray = Mathf.Min(m_LooSpray, MaxLooStat);

                m_ScaleValues.AnalSprayAmount = m_AnalSprayAmount;

                // m_SoundSpray?.Play(); 
                m_Sounds[6].Play();
                m_Sounds[UnityEngine.Random.Range(7, 9)].Play();
                m_Anus.GetComponent<Turret>().Fire();

                SaveStats();
            }
            
        }

     


        public void RestoreHP(int hp)
        {
          
            petAsSpaceShip.RestoreHitPoints(Mathf.Min(petAsSpaceShip.MaxHitPoints - petAsSpaceShip.HitPoints, hp));
        }

       
        #endregion



        public void Eat(InventoryItem food)
        {
            Inventory.Instance.AddItemInstances(food, -1);
            m_FoodHunger = Mathf.Min(m_FoodHunger + food.foodBonus, MaxStat);


            SaveStats();
        }

        public void OhPoop()
        {
            m_LooPoo = 0;
        }

        private void InitToiletStatArray()
        {
            m_ToiletStats = new float[5];
            m_ToiletStats[0] = m_LooPoo;
            m_ToiletStats[1] = m_LooSpray;
            m_ToiletStats[2] = m_LooPee;
            m_ToiletStats[3] = m_Bath;
            m_ToiletStats[4] = m_BrushTeeth;
        }

        public float TakeToiletStat(int index)
        {
            InitToiletStatArray();
            return m_ToiletStats[index];    
        }

        public void CareToiletStat(ToiletStat stat, float value)
        {
            if (stat == ToiletStat.Poo)
            {
                m_LooPoo += value;
                m_LooPoo = Mathf.Clamp(m_LooPoo, 0, MaxLooStat);
            }
            if (stat == ToiletStat.AnalSpray)
            {
                m_AnalSprayAmount -= value;
                m_AnalSprayAmount = Mathf.Clamp(m_AnalSprayAmount, 0, AnalGlandVolume);
            }
            if (stat == ToiletStat.Pee)
            {
                m_LooPee += value;
                m_LooPee = Mathf.Clamp(m_LooPee, 0, MaxLooStat);
            }
            if (stat == ToiletStat.Bath)
            {
                m_Bath += value;
                m_Bath = Mathf.Clamp(m_Bath, 0, MaxBath);
            }
            if (stat == ToiletStat.BrushTeeth)
            {
                m_BrushTeeth += value;
                m_BrushTeeth = Mathf.Clamp(m_BrushTeeth, 0, MaxBrushTeeth);
            }

            InitToiletStatArray();
            SaveStats();
        }

        public void SetToiletStats(float poo, float analSpray, float pee, float bath, float brushTeeth)
        {            
                m_LooPoo = Mathf.Clamp(poo, 0, MaxLooStat);
            
                m_AnalSprayAmount = Mathf.Clamp(analSpray, 0, AnalGlandVolume);
            
           
                m_LooPee = Mathf.Clamp(pee, 0, MaxLooStat);
            
                m_Bath = Mathf.Clamp(bath, 0, MaxBath);
            
                m_BrushTeeth = Mathf.Clamp(brushTeeth, 0, MaxBrushTeeth);
            

                InitToiletStatArray();
                SaveStats();
        }

      
    }

}
