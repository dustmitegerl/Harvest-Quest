using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour //Thanks to Game Dev with JacquelynneHei https://youtu.be/1NTNIm0tcXw?si=e01uhzdpnzFTxu-q
{
    [SerializeField] private Tilemap interactableMap; 
    //Tiles that will be used to farm
    [SerializeField] private Tile hiddenInteractableTile;
    //Hidden tiles that will not be seen
    [SerializeField] private Tile interactedTile;
    //Tiles that will be seen after interaction with ground
    void Start()
    {//Will not see tiles that player will interact with
        foreach(var position in interactableMap.cellBounds.allPositionsWithin)
        {
            interactableMap.SetTile(position, hiddenInteractableTile);
        }
    }

    public bool IsInteractable(Vector3Int position)
    {//Should show
        TileBase tile = interactableMap.GetTile(position);

        if(tile != null)
        {
            if(tile.name == "Interactable")
            {
                return true;
            }
        }

        return false;
    }

    public void SetInteracted(Vector3Int position)
    {
        interactableMap.SetTile(position, interactedTile);
    }
}
