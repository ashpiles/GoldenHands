->Main


LIST DR_jewler_suspicion = pot, stairs, crates, case, no_suspicion
LIST DR_jewler_rapport = daughter, gossip, marriage
LIST DR_jewler_states = baiting, neutral, on_edge, unaware

VAR jewler_rapport = ()
VAR jewler_suspicion = no_suspicion
VAR jewler_state = neutral


/*

Before we enter the shop we explain there is an explosion rigged next door as a distraction

begins with the player being welcomed into the shop this is our tutorial

Before we enter the

The player can check the crates
flower pot
*/

===Main===
->Intro

===Intro===
# start_filter
You are a dull well-breed young lady.
Dressed in the latest fashion strolling up and down this street. Nearly a care between each step walking to each window, naturally peering in windows to lazily gaze on fineries and fancies.
So naturally nauseating the way luster glazes over the eyes...
And so long as you play the part the, con will go off without a hitch. Everything is set up like how they said.
A freak gas leak will go off in the next building just in time for me to swap the dummy diamond necklace.
Just find it and swap it during the havoc. What could possibly go wrong.
# break_filter
->DisplayRoom

= dr_jewler
Jewler: Welcome in.
Jewler: ... How can I help you.
* Ask him how he's doing -> dr_c1
* Make small talk about the weather -> dr_c2
* Where is the most expensive jewels, I got cash to spend -> dr_c3
+ I think I'm just going to look around-> DONE

= dr_c1
Jewler: ... Alright I supppose. Slow day, is there anything in particular you are looking for? I was about to close up shop early today.
->dr_jewler

= dr_c2
Jewler: Yeah, its swealtering I don't see how you're out in a dress like that today.

* I don't know either... -> DisplayRoom
* Berate his lack of fashion sense -> dr_c2_b

= dr_c2_b
Player: For a jewler you sure don't know the trends, you'd think having so many pretty ladies walking by your shop you'd recognize a few!

Jewler: A fair point. Though not many people come by my shop, its a bit out of the way though I should take some notes, my daughter's birthday is coming up soon.
Say, any reccommendations for a girl a bit younger than yourself.

* One million dollars
    Jewler: ... Oh, hahah very funnny. -> dr_jewler
* A very very very big diamond
    Jewler: Hahaha well if there's any perk to a jewler as a father I guess that would be the one.
    Player: Wouldn't it! Man I'm sure jealous of her to have such a kind father. I bet you picked out a real rare one just for her.
    Jewler: Well now don't be spoiling her birthday gift early for her.
    ~ DR_jewler_rapport += daughter
    -> dr_jewler
* Maybe a nice dress
    Jewler: Hmm I'll have to ask the tailor across the street for a reccommendation then.
    -> dr_jewler

->DONE

= dr_c3
{ jewler_rapport:
- daughter:
    Jewler: Hahaha, you have quite the sense of humour little lady. Though just between you and me I got a very expensive diamond necklace today. Though I fear its reserved for a different client
- else: 
    {  - jewler_suspicion != no_suspicion :
            Jewler: ... I would have to go check the books. I'm afraid I don't know these things off the top of my head. 
            You stay right here. I'll go check the records.
            ~ jewler_state = baiting
            -> DisplayRoom
        - else:
            Jewler: Recently we had a shipment of some equisite silver necklaces you could look at. It might be more around your price range.
            ->dr_jewler
    }
}
    

->DisplayRoom
===DisplayRoom===

= dr_stairs
{  jewler_state == baiting:
    Where do you think you are going, thief.
    - else:
        {  jewler_suspicion == stairs:
                Jewler: I strongly reccommend you remove yourself from those stairs. Or I will regretfully be forced to remove you from my shop.
                ~ jewler_state = on_edge
            - else:
               Jewler: I'm sorry but that area is off limits, I'll have to ask for you to step away from the stairs.
                ~ jewler_suspicion += stairs
        }
     
}
->DONE

= dr_pot
-> DONE
= dr_crate
->DONE
= dr_case
->DONE


