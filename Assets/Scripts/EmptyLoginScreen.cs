using UnityEngine;
using TMPro;

public class EmptyLoginScreen : MonoBehaviour
{
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;
    private void OnEnable()
    {
        email.text = "";
        password.text = "";
    }
}
