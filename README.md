# TraitLearner
A utility mod for VintageStory that allows learning new Traits. No Recipes get added because i can't judge how your server wants to balance this out (only dungeon loot, craftable, etc.).
It adds Commands and a CollectibleBehavior to add/remove Traits (and apply stat bonus/malus) and adds an overview of extra Traits to the character-tab.

#<h1 style="color:red">NO LONGER MAINTAINED</h1>
This mod is no longer maintained. If you have interest on taking it over reach out to me on github or via the vintage story discord. 
<h1 style="color:red">NO LONGER MAINTAINED</h1>



![TraitLearner-screenshot](https://gitlab.com/P3t3rix/vstraitlearner/uploads/a9fa0534e38f2a32d3e0a5410bde9d2f/image.png)

## Commands
The following Commands are added [] are needed, () are optional, everything except the status command needs admin permission:

- show all traits of your player:
  `/traitlearner status`
- add a trait
  `/traitlearner add [TraitName] (playerName)`
- remove a trait
  `/traitlearner remove [TraitName] (playerName)`
- reset traits so only character traits remain
  `/traitlearner reset (playerName)`


## Behaviors

The following CollectibleBehavior gets added:


```
{
  "name": "ManageTrait",
  "properties": {
    "traits_add": [ "XXX", "YYY" ], //traits to add when using the item
    "traits_remove": [ "ZZZ", "AAA" ], //traits to remove when using the item
    "usage_duration_in_seconds" : 4, //how long do i have to use the item before it is used
    "reset" : false, //if the item should reset the traits (will ignore traits_add, traits_remove) 
  }
}
```


For examples of items that add or remove traits look in the examples folder.
