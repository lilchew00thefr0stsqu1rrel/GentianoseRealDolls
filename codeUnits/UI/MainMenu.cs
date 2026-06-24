using TowerDefense;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace GentianoseRealDolls
{
    public class MainMenu : MonoBehaviour
    {
        [Inject]
        public void Construct(TeleportBeasts teleportBeasts)
        {
            m_TeleportBeasts = teleportBeasts;
        }

        [SerializeField] private TeleportBeasts m_TeleportBeasts;

        [SerializeField] private VersionData m_Version;
        [SerializeField] private Text m_VersionText;
        [SerializeField] private Text m_DebugText;

        [SerializeField] private GameObject m_ButtonsPanel;
        [SerializeField] private GameObject m_HelpPanel;
        [SerializeField] private int m_LocationIndex;
        public const string pathScene = "Assets/JSON/scene_map.dat";

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_VersionText.text = $"v. {m_Version.Domain} {m_Version.Major}." +
                $"{m_Version.Minor}.{m_Version.Micro}";



            m_DebugText.text = "Иб";
            PlayerPrefs.SetString("~", "Ибис");
            PlayerPrefs.Save();
            m_DebugText.text = PlayerPrefs.GetString("~", "ЭД");

            m_HelpPanel.SetActive(false);
            ReadCurrScene();


            
            m_DebugText.text = PlayerPrefs.GetString(WhooSettings.fileNameInv, "Grison goes chk-chk");
            
   
        }
        public void ReadCurrScene()
        {
            Saver<int>.TryLoad(WhooSettings.fileNameLoc, ref m_LocationIndex);

            
            m_DebugText.text = m_LocationIndex.ToString();
        }

        [SerializeField] private GameObject m_Visual;
        public void ToHabitat()
        {
            m_Visual.SetActive(false);
            ReadCurrScene();
            print("LocInd " + m_LocationIndex);


            m_TeleportBeasts.EnterScene(m_LocationIndex);
        }



        public void Quit()
        {
            Application.Quit();
        }
        public void OpenHelp()
        {
            m_ButtonsPanel.SetActive(false);
            m_HelpPanel.SetActive(true);
        }
        public void CloseHelp()
        {
            m_HelpPanel.SetActive(false);
            m_ButtonsPanel.SetActive(true);
        }
    }
}

