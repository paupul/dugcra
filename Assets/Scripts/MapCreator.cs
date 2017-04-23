using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapCreator : MonoBehaviour
{
    public ToggleGroup placables;
    public GameObject markerPrefab;
    public LayerMask mask;
    public LevelManager manager;

    void Start()
    {

    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        horizontal = horizontal * 5 * Time.deltaTime;
        vertical = vertical * 5 * Time.deltaTime;

        Vector3 position = new Vector3(transform.position.x + horizontal, transform.position.y + vertical, transform.position.z);

        transform.position = position;

        if (placables.gameObject.activeInHierarchy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (placables.AnyTogglesOn())
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        Text text = placables.ActiveToggles().FirstOrDefault().transform.GetChild(0).GetComponent<Text>();
                        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        GridTile tile = LevelManager.SpawnedWorldComp.GetGrid(pos).GetTile(pos);
                        tile.type = GridTile.TileTypes.Ground;
                        switch (text.text)
                        {
                            case "Starting point":
                                tile.containedObject = GridTile.ContainedObject.StartingPoint;
                                LevelManager.SpawnedWorldComp.startingPoint = new WorldPos((int)pos.x, (int)pos.y);
                                LevelManager.SpawnedWorldComp.startingGrid = new WorldPos((int)pos.x, (int)pos.y);
                                break;
                            case "Ladder":
                                tile.containedObject = GridTile.ContainedObject.Ladder;
                                break;
                            case "Pit":
                                tile.containedObject = GridTile.ContainedObject.Pit;
                                break;
                            case "Spear":
                                tile.containedObject = GridTile.ContainedObject.Spear;
                                break;
                            case "Enemy":
                                tile.containedObject = GridTile.ContainedObject.Enemy;
                                break;
                            case "Chest":
                                tile.containedObject = GridTile.ContainedObject.Chest;
                                break;
                            case "Wall":
                                tile.containedObject = GridTile.ContainedObject.Empty;
                                tile.type = GridTile.TileTypes.Wall;
                                break;
                            case "Ground":
                                tile.containedObject = GridTile.ContainedObject.Empty;
                                break;
                            default:
                                break;
                        }
                        tile.itemSpawned = false;
                        LevelManager.SpawnedWorldComp.GetGrid(pos).update = true;
                        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        var end = mouse;
                        end.z = 1;
                        RaycastHit2D hit;
                        if (hit = Physics2D.Linecast(mouse, end, mask))
                        {
                            if (hit.collider != null)
                            {
                                Destroy(hit.collider.gameObject);                                
                            }
                        }
                        manager.Spawn(LevelManager.SpawnedWorldComp.GetGrid(pos));
                    }
                }
            }
        }
    }
}
