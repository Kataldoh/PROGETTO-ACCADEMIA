using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMethods : MonoBehaviour
{

    MainPlayerScript pInst = MainPlayerScript.pInstance;
    // Start is called before the first frame update
    bool isShoot;   //controlla se è stato sparato un colpo
    bool isAiming;  //controlla se si sta mirando
    GameObject enemy, oneShot;   //salva il nemico colpito
    int i;  //projectile Index
    float shootingInterval; //usato per controllare l'intervallo di sparo
        
    public void GeneralWeaponHandler(WeaponStats wS, Transform aimStart, GameObject[] trailGO)
    {
        //da la posizione del mouse rispetto al punto iniziale dato
        Vector3 direction = _vectorDir(aimStart);
        
        //controllo se l'indice non va oltre la lunghezza dell'array dei proiettili
        if(i >= trailGO.Length)
        {
            i=0;
        }

        //metodo di sparo
        if(Input.GetButton("Fire1") && !isShoot && isAiming)
        {
            isShoot = true;
            shootingInterval = 0;
            trailGO[i].SetActive(true);
            trailGO[i].transform.position = aimStart.position;
            RaycastHit hit;
            if (Physics.Raycast(aimStart.position, direction, out hit, 5))   //Se il raycast colpisce qualcosa 
            {
                pInst.lastShotPosition = hit.point; 
                if(hit.collider.tag == "Nemico")            //Se colpisce un nemico
                {
                    enemy=hit.collider.gameObject;          //Assegno a enemy il nemico da uccidere a fine del delay
                }

                if (hit.collider.tag == "OneShot")            //Se colpisce un nemico
                {
                    oneShot = hit.collider.gameObject;          
                }
            }
            else
            {
                pInst.lastShotPosition = aimStart.position + direction * 5;
            }
        }

        print(isShoot);

        //se si spara
        if(isShoot)
        {
            trailGO[i].transform.position = Vector3.Lerp(trailGO[i].transform.position, pInst.lastShotPosition, Time.deltaTime * 10);   //imposta la posizione del trailGO puntato
            shootingInterval += Time.deltaTime;
            if (shootingInterval >= wS.shootingInterval_inSeconds)      //se l'intervallo di sparo è maggiore o uguale a quello dell'arma
            {
                if(enemy != null)
                {
                    if (!enemy.GetComponent<HealthPlaceholder>().hit)
                    {
                        enemy.GetComponent<HealthPlaceholder>().hit = true;
                        enemy.GetComponent<HealthPlaceholder>().damage = wS.damage;
                    }
                }
                
                if(oneShot != null)
                {
                    Destroy(oneShot);
                }


                trailGO[i].transform.position = aimStart.position;      //Resetta la posizione del trailGO/Proiettile
                trailGO[i].gameObject.SetActive(false);                 //lo porta a falso
                isShoot= false;
                i++;
            }
        }
    }

    public Vector3 ScreenAiming(Transform aimStart)
    {
        //calcolo della direzione dello sparo
        Vector3 direction = _vectorDir(aimStart);
        Vector3 mousePos = new Vector3(Input.mousePosition.x - Screen.width/2,
                            Input.mousePosition.y - Screen.height/2, Input.mousePosition.z);

        //(transform.position - cursor.position).normalized;
        //print(transform.position - cursor.position);
        //var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        Debug.DrawRay(aimStart.position, direction * 5, Color.red);  //disegno il raggio nella scena

        //posizionamento del laser di mira
        pInst.laserRender.SetPosition(0, aimStart.position);
        pInst.laserRender.SetPosition(1, aimStart.position + direction * 5);

        //Se tengo premuto tasto destro del mouse, si inizierà a "sparare"
        if (Input.GetButton("Fire2"))
        {
            isAiming=true;                              //mette se si sta mirando a vero
            pInst.dirX = Mathf.Round(direction.x);      //cambia la direzione del player
            pInst.laserRender.enabled = true;           //rende il laser di mira visibile
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
            isAiming=false;                     
            pInst.dirX = 0;                     //resetta la rotazione del player
            pInst.laserRender.enabled = false;  //disattiva il laser
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
