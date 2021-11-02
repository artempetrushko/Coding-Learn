using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Crux.Utility
{
    public class CruxMenu : MonoBehaviour
    {
        [MenuItem("Window/Crux/Create/Create New Crux System", false, 1)]
        static void CreateNewSystem()
        {
            GameObject CruxSystem = (GameObject)Instantiate(Resources.Load("New Crux System"), new Vector3(0, 0, 0), Quaternion.identity);
            CruxSystem.transform.position = new Vector3(0, 0, 0);
            CruxSystem.gameObject.name = "Crux";
        }

        [MenuItem("Window/Crux/Documentation/Documentation", false, 100)]
        static void CruxDocumentationHome()
        {
            Application.OpenURL("https://docs.google.com/document/d/1Ee0eqh9kzGXK_2IrcXBL3KpujmGctSgPkFkLB4HjR4w/edit?usp=sharing");
        }

        [MenuItem("Window/Crux/Documentation/Video Tutorials", false, 100)]
        static void CruxDocumentationTutorials()
        {
            Application.OpenURL("https://www.youtube.com/playlist?list=PLlyiPBj7FznasueQUe7_51nNvGkUuu7B1");
        }

        [MenuItem("Window/Crux/Support", false, 100)]
        static void CruxDocumentationSupport()
        {
            Application.OpenURL("http://www.blackhorizonstudios.com/contact/");
        }
    }
}