-- This function will be called by C# when the player interacts

object = {
    x = 100,
    y = 100,
    atlas = "images/dev_assets/t1_atlas.xml",
    sprite = "vendor1-animation",
    hitbox_color = "#0000FF19",  
    debug_enabled = true;
    physics_type = "Solid"
}

dialogue = {
    
    start = {
        speaker = "Vendor",
        text = " Welcome!\n I wish I had more to offer but this\n is a bit of a work in progress.\n Please, make yourself at home!",
        next = "start2",
        position = "bottom"

    },
     start2 = {
        speaker = "Vendor",
        text = " Debugging is pretty hard.",
        next = "intro",
        position = "bottom"
    },
    intro = {
        speaker = "Vendor",
        text = " Are you excited to see what's next?",
        choices = {
            { text = "Yes!", next = "testYes" },
            { text = "No way!", next = "testNo" },
        }  ,
        position = "top" 
    },
    testYes = {
        speaker = "Vendor",
        text = " That's the spirit!",
        next = "end_dialogue",
        position = "bottom"
    },
    testNo = {
        speaker = "Vendor",
        text = " Well, hopefully you'll be singing\n a different tune after some updates!",
        next = "end_dialogue",
        position = "bottom"
    },
    end_dialogue = {
        speaker = "END",
        text =  "END",
        next = "END",
        position = "bottom" 
    },
    END = {
        speaker = "END",
        text =  "END",
        next = "END",
        position = "bottom"
    }
}
function onInteract()
    printMessage("Interaction Successful")
end