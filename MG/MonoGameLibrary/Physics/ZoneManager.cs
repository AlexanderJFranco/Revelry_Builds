using System;
using System.Collections.Generic;

namespace MonoGameLibrary.Physics;
/*Summary
The ZoneManager class tracks all RectZone objects in the current scene. The List of Zones will contain (RectZone, Interactable) so that
the manager can track if a zone can be interacted with.
*/
public class ZoneManager
{

    //Constructor
    private List<(RectZone, Interactable? obj)> zones = new();

    //Add Zone to list
    public void RegisterZone(RectZone zone, Interactable? obj = null)
    {
        zones.Add((zone, obj));
    }

    //Remove Zone from List
    public void UnregisterZone(RectZone zone, Interactable? obj = null)
    {
        zones.Remove((zone, obj));
    }

    //Return list of all zones currently loaded into Manager
    public List<(RectZone zone, Interactable? obj)> GetZones()
    {
        return zones;
    }

    public void OutputZones()
    {
        foreach (var (zone, obj) in zones)
        {
            Console.WriteLine("Object: " + obj +" X: " + zone.X + " Y: " + zone.Y + " Width: " + zone._width + " Height: " + zone._height);
        }
    }
    //Check if an object has collided with a Non-Passable Zone - Walls, Obstacles, etc.
    public bool CheckCollision(RectZone testZone, out RectZone collidedZone)
    {
        foreach (var (zone, _) in zones)
        {
            if (zone._zoneType == ZoneType.Solid && zone.Intersects(testZone))
            {
                collidedZone = zone;
                return true;
            }
        }
        collidedZone = default;
        return false;
    }

    //Check if the object being intersected is an Interactive object
    public Interactable? CheckInteractions(RectZone playerInteractZone)
    {
        //Iterate over each zone and object pair within Zone manager
        foreach (var (zone, obj) in zones)
        {
            //Check for overlap
            if (playerInteractZone.Intersects(zone))
            {
                //Return object being interacted with
                return obj;
            }
        }
        return null;
    }



}