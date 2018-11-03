using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponContainer))]
public class WeaponContainerEditor : Editor {

    public override void OnInspectorGUI()
    {
        WeaponContainer weaponContainer = (WeaponContainer)target;
        weaponContainer.weaponType = (WeaponContainer.WeaponType)EditorGUILayout.EnumPopup("Weapon Type", weaponContainer.weaponType);
        if(weaponContainer.weaponType == WeaponContainer.WeaponType.Sword)
        {

            weaponContainer.weaponLevel = EditorGUILayout.IntField("Weapon Damage", weaponContainer.weaponLevel);
            weaponContainer.rarity = (Weapon.Rarity)EditorGUILayout.EnumPopup("Rarity", weaponContainer.rarity);
            weaponContainer.weaponWeight = EditorGUILayout.IntField("Weight", weaponContainer.weaponWeight);
            weaponContainer.attackRate = EditorGUILayout.FloatField("Attack rate", weaponContainer.attackRate);


        }
        if(weaponContainer.weaponType == WeaponContainer.WeaponType.Bow)
        {
            weaponContainer.projectileHandler = (GameObject)EditorGUILayout.ObjectField("Projectile handler", weaponContainer.projectileHandler, typeof(GameObject), true);
            weaponContainer.ammoType = (GameObject)EditorGUILayout.ObjectField("Ammo", weaponContainer.ammoType, typeof(GameObject), false);
            weaponContainer.rarity = (Weapon.Rarity)EditorGUILayout.EnumPopup("Rarity", weaponContainer.rarity);
            weaponContainer.weaponLevel = EditorGUILayout.IntField("Weapon damage", weaponContainer.weaponLevel);
            weaponContainer.attackRate = EditorGUILayout.FloatField("Firerate", weaponContainer.attackRate);
            weaponContainer.weaponWeight = EditorGUILayout.IntField("Weight", weaponContainer.weaponWeight);
            weaponContainer.weaponRange = EditorGUILayout.IntField("Weapon range", weaponContainer.weaponRange);

        }
        if (GUI.changed) { EditorUtility.SetDirty(weaponContainer); }

    }

}
