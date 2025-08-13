-- This function will be called by C# when the player interacts

object = {
    x = 100,
    y = 100,
    atlas = "images/dev_assets/t1_atlas.xml",
    sprite = "vendor1-animation",
    hitbox_color = "#0000FF19",  
    debug_enabled = true;
    musicTrack = "lounge_theme"
}


function onInteract()
    printMessage("Interaction Successful")
end