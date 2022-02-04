using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMethods : MonoBehaviour
{

    MainPlayerScript pInst = MainPlayerScript.pInstance;
    // Start is called before the first frame update

        
    public void GeneralWeaponHandler(Transform aimStart, TrailRenderer trailR)
    {
        Vector3 direction = _vectorDir(aimStart);
        
        if(Input.GetButtonDown("Fire1"))
        {
            trailR.gameObject.SetActive(true);
            RaycastHit hit;
            print("In here");
            if (Physics.Raycast(aimStart.position, direction, out hit, 5))   //Se il raycast colpisce qualcosa 
            {
                trailR.transform.position = Vector3.Lerp(aimStart.position, hit.point, Time.deltaTime/2);
                if(hit.collider.tag == "Nemico")            //Se colpisce un nemico
                {
                    Destroy(hit.collider.gameObject);
                }
            }
            else
            {
                trailR.transform.position = Vector3.Lerp(aimStart.position, new Vector3(Input.mousePosition.x - Screen.width/2 -aimStart.position.x,
                            Input.mousePosition.y - Screen.height/2 -aimStart.position.y, Input.mousePosition.z), Time.deltaTime/2);
            }
        }
    }

    public Vector3 ScreenAiming(Transform aimStart)
    {
        //calcolo della direzione dello sparo
        Vector3 direction = _vectorDir(aimStart);
        Vector3 mousePos = new Vector3(Input.mousePosition.x - Screen.width/2 -aimStart.position.x,
                            Input.mousePosition.y - Screen.height/2 -aimStart.position.y, Input.mousePosition.z);

        //(transform.position - cursor.position).normalized;
        //print(transform.position - cursor.position);
        //var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        Debug.DrawRay(aimStart.position, direction * 5, Color.red);  //disegno il raggio nella scena

        pInst.laserRender.SetPosition(0, aimStart.position);
        pInst.laserRender.SetPosition(1, mousePos);

        //Se tengo premuto tasto destro del mouse, si inizierà a "sparare"
        if (Input.GetButton("Fire2"))
        {
            pInst.dirX = Mathf.Round(direction.x);
            pInst.laserRender.enabled = true;
            RaycastHit hit;
            if (Physics.Raycast(aimStart.position, direction, out hit, 5))   //Se il raycast colpisce qualcosa 
            {
                pInst.laserRender.SetPosition(1, hit.point);         //Disegna la fine del raggio sul punto colpito
                //print("Hit");
                if(hit.collider.tag == "Nemico")            //Se colpisce un nemico
                {
                    print("Hit Enemy");
                }
                return hit.point;
            }
        }
        else
        {
            pInst.dirX = 0;
            pInst.laserRender.enabled = false;
        }

        return mousePos;
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