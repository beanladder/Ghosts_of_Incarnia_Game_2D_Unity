using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : MonoBehaviour
{
    int activeIndexNum=0;
    private BobbyDeol playerControls;
    private void Awake(){
        playerControls = new BobbyDeol();
    }
    private void Start(){
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
        ToggleActiveHighLight(0);
    }
    private void OnEnable(){
        playerControls.Enable();
    }
    private void ToggleActiveSlot(int numvalue){
        ToggleActiveHighLight(numvalue-1);
    }
    void ToggleActiveHighLight(int indexNum){
        activeIndexNum = indexNum;
        foreach(Transform inventorySlot in this.transform){
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }
        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);
        ChangeActiveWeapon();
    }
    private void ChangeActiveWeapon(){
        if(ActiveWeapon.Instance.CurrentActiveWeapon!=null){
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }
        if(!transform.GetChild(activeIndexNum).GetComponentInChildren<InventorySlot>()){
            ActiveWeapon.Instance.WeaponNull();
            return;
        }
        GameObject WeaponToSpawn = transform.GetChild(activeIndexNum).GetComponentInChildren<InventorySlot>().GetWeaponInfo().weaponPrefab;
        GameObject newWeapon = Instantiate(WeaponToSpawn,ActiveWeapon.Instance.transform.position,Quaternion.identity);
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0,0,0);

        newWeapon.transform.parent = ActiveWeapon.Instance.transform;
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
