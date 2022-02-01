using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMethods : MonoBehaviour
{

    MainPlayerScript pInst = MainPlayerScript.pInstance;
    // Start is called before the first frame update

        
    public void GeneralWeaponHandler(WeaponStats wS, Transform aimStart)
    {
        Vector3 direction = _vectorDir(aimStart);

        if(Input.GetButton("Fire1"))
        {
            RaycastHit hit;
            
            if (Physics.Raycast(aimStart.position, direction, out hit, 5))   //Se il raycast colpisce qualcosa 
                {
                    if(hit.collider.tag == "Nemico")            //Se colpisce un nemico
                    {
                        
                    }
                }
        }
    }

    public void ScreenAiming(Transform aimStart)
    {
        //calcolo della direzione dello sparo
            Vector3 direction = _vectorDir(aimStart);

            //(transform.position - cursor.position).normalized;
            //print(transform.position - cursor.position);
            //var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            Debug.DrawRay(aimStart.position, direction * 5, Color.red);  //disegno il raggio nella scena

            pInst.laserRender.SetPosition(0, aimStart.position);
            pInst.laserRender.SetPosition(1, new Vector3(Input.mousePosition.x - Screen.width/2 -aimStart.position.x,
                             Input.mousePosition.y - Screen.height/2 -aimStart.position.y, Input.mousePosition.z));

            //Se tengo premuto tasto destro del mouse, si inizierà a "sparare"
            if (Input.GetButton("Fire2"))
            {
                pInst.dirX = Mathf.Round(direction.x);
                pInst.laserRender.enabled = true;
                RaycastHit hit;
                if (Physics.Raycast(aimStart.position, direction, out hit, 5))   //Se il raycast colpisce qualcosa 
                {
                    pInst.laserRender.SetPosition(1, hit.point);         //Disegna la fine del raggio sul punto colpito
                    print("Hit");
                    if(hit.collider.tag == "Nemico")            //Se colpisce un nemico
                    {
                        print("Hit Enemy");
                    }
                }
            }
            else
            {
                pInst.laserRender.enabled = false;
            }
    }

    Vector3 _vectorDir(Transform aimStart) 
    {
        var vettoredir = new Vector3(Input.mousePosition.x - Screen.width/2 -aimStart.position.x,
                             Input.mousePosition.y - Screen.height/2 -aimStart.position.y, Input.mousePosition.z).normalized;
        var dist = vettoredir.magnitude;
        Vector3 direction = (vettoredir / dist);
        direction.z = 0;
        return direction;
    }
}
