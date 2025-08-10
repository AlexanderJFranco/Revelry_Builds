-- This function will be called by C# when the player interacts

object = {
    x = 50,
    y = 50,
    atlas = "images/dev_assets/t1_atlas.xml",
    sprite = "player1-animation",
    musicTrack = "lounge_theme"
}


function onInteract()
    printMessage("Interaction Successful")
end