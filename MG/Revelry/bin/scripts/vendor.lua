-- This function will be called by C# when the player interacts

object = {
    x = 100,
    y = 100,
    atlas = "images/dev_assets/t1_atlas.xml",
    sprite = "vendor1-animation",
    hitbox_color = "#0000FF19",  
    debug_enabled = false;
    physics_type = "Solid"
}

dialogue = {
    
    start = {
        speaker = "Guardian",
        text = " Welcome!\n I wish I had more to offer but this\n is a bit of a work in progress.\n Please, make yourself at home!",
        next = "intro"
    },
    intro = {
        speaker = "Guardian",
        text = " More to come!",
        next = "end_dialogue"
            
    },
    end_dialogue = {
        speaker = "END",
        text =  "END",
        next = "END", 
    }
}
function onInteract()
    printMessage("Interaction Successful")
end