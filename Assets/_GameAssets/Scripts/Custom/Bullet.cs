using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnEnable() {
        Debug.Log("BulletRot : "+transform.localEulerAngles);
    }
    private void OnDisable() {
        transform.rotation = Quaternion.identity;
        
    }

}