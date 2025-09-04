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
        speaker = "Vendor",
        text = " Welcome!\n I wish I had more to offer but this\n is a bit of a work in progress.\n Please, make yourself at home!",
        next = "testYes"
    },
    intro = {
        speaker = "Vendor",
        text = " Are you excited to see what's next?",
        choices = {
            { text = "Yes!", next = "testYes" },
            { text = "No way!", next = "testNo" }
        }   
    },
    testYes = {
        speaker = "Vendor",
        text = " That's the spirit!",
        next = "end_dialogue"
    },
    testNo = {
        speaker = "Vendor",
        text = " Well, hopefully you'll be singing\n a different tune after some updates!",
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