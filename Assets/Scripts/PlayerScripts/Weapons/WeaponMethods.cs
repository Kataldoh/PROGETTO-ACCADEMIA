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
    Vector3 shotStartPosition, maxShotPosition;
    GameObject shotParticle = null;

    public void GeneralWeaponHandler(WeaponStats wS, Transform aimStart, GameObject shootHit, GameObject[] trailGO,LayerMask ignoreLayer)
    {
        //da la posizione del mouse rispetto al punto iniziale dato
        Vector3 direction = _vectorDir(aimStart);
        
        //controllo se l'indice non va oltre la lunghezza dell'array dei proiettili
        if(i >= trailGO.Length)
        {
            i=0;
        }

        //metodo di mira al quale servono 
        ScreenAiming(aimStart, ignoreLayer);

        //metodo di sparo
        if (Input.GetButton("Fire1") && !isShoot)
        {
            isShoot = true;
            shootingInterval = 0;
            trailGO[i].SetActive(true);
            trailGO[i].transform.position = aimStart.position;
            trailGO[i].GetComponent<TrailRenderer>().Clear();
            RaycastHit hit;
            shotStartPosition = aimStart.position;
            

            if (!isAiming) 
            {
                direction = aimStart.forward;
                direction.z = 0;
            }

            maxShotPosition = aimStart.position + direction * wS.weaponRange;

            if (Physics.Raycast(aimStart.position, direction, out hit, wS.weaponRange, ~ignoreLayer))   //Se il raycast colpisce qualcosa 
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
                pInst.lastShotPosition = maxShotPosition;
            }

            
        }

        //print(isShoot);

        //se si spara
        if(isShoot)
        {
            trailGO[i].transform.position = Vector3.Lerp(trailGO[i].transform.position, maxShotPosition, Time.deltaTime * wS.weaponRange * wS.shootingInterval_inSeconds * 10);   //imposta la posizione del trailGO puntato
            shootingInterval += Time.deltaTime;

            if(Vector3.Distance(trailGO[i].transform.position, pInst.lastShotPosition) <= 0.1f || shootingInterval >= wS.shootingInterval_inSeconds)
            {
                if (enemy != null)
                {
                    if (!enemy.GetComponent<HealthPlaceholder>().hit)
                    {
                        enemy.GetComponent<HealthPlaceholder>().hit = true;
                        enemy.GetComponent<HealthPlaceholder>().damage = wS.damage;
                        enemy = null;
                    }
                }

                if (oneShot != null)
                {
                    Destroy(oneShot);
                }

                if(shotParticle == null)
                    shotParticle = Instantiate(shootHit, trailGO[i].transform.position, trailGO[i].transform.rotation);

                trailGO[i].transform.position = aimStart.position;      //Resetta la posizione del trailGO/Proiettile
                trailGO[i].gameObject.SetActive(false);                 //lo porta a falso
            }
                

            if (shootingInterval >= wS.shootingInterval_inSeconds)      //se l'intervallo di sparo è maggiore o uguale a quello dell'arma
            {
                shotParticle = null;
                isShoot = false;
                i++;
            }
        }
    }

    public Vector3 ScreenAiming(Transform aimStart, LayerMask ignoreLayer)
    {
        //calcolo della direzione dello sparo
        Vector3 direction = _vectorDir(aimStart);

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
            if (Physics.Raycast(aimStart.position, direction, out hit, 5, ~ignoreLayer))   //Se il raycast colpisce qualcosa 
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

        return direction;
    }

    Vector3 _vectorDir(Transform aimStart) 
    {
        var vettoredir = new Vector3(Input.mousePosition.x -Camera.main.WorldToScreenPoint(aimStart.position).x,
                             Input.mousePosition.y -Camera.main.WorldToScreenPoint(aimStart.position).y, Input.mousePosition.z).normalized;
        var dist = vettoredir.magnitude;
        Vector3 direction = (vettoredir / dist);
        direction.z = 0;
        return direction;
    }
}
