using UnityEngine;
using UnityEngine.UI;
 
public class glug : MonoBehaviour {
 
    public void SetVolume( Slider s)
    {
        Debug.Log( "fnord:" + s.value);
    }
}