using System;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinecraftTextureBuilder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region variables

        //Strings

        string CurrentVersion = "1.2";

        string BaseURL = "http://pckstudio.tk/";

        string BackURL = "https://phoenixarc.github.io/pckstudio.tk/";

        string updatePath= "studio/Texture/api/TextureToolUpdate.txt";

        string workingDir = Environment.CurrentDirectory + "\\InputTextures";

        string[,] ItemSheetArray =
        {
        {"leather_helmet","chainmail_helmet","iron_helmet","diamond_helmet","golden_helmet","flint_and_steel","flint","coal","string","wheat_seeds","apple","golden_apple","egg","sugar","snowball","elytra" },
        {"leather_chestplate","chainmail_chestplate","iron_chestplate","diamond_chestplate","golden_chestplate","bow","brick","iron_ingot","feather","wheat","painting","sugarcane","bone","cake","slime_ball","broken_elytra" },
        {"leather_leggings","chainmail_leggings","iron_leggings","diamond_leggings","golden_leggings","arrow","end_crystal","gold_ingot","gunpowder","bread","oak_sign","oak_door","iron_door","","fire_charge","chorus_fruit" },
        {"leather_boots","chainmail_boots","iron_boots","diamond_boots","golden_boots","stick","compass_00","diamond","redstone","clay_ball","paper","book","map","pumpkin_seeds","melon_seeds","popped_chorus_fruit" },
        {"wooden_sword","stone_sword","iron_sword","diamond_sword","golden_sword","fishing_rod","clock_00","bowl","mushroom_stew","glowstone_dust","bucket","water_bucket","lava_bucket","milk_bucket","ink_sac","gray_dye" },
        {"wooden_shovel","stone_shovel","iron_shovel","diamond_shovel","golden_shovel","fishing_rod_cast","repeater","porkchop","cooked_porkchop","cod","cooked_cod","rotten_flesh","cookie","shears","red_dye","pink_dye" },
        {"wooden_pickaxe","stone_pickaxe","iron_pickaxe","diamond_pickaxe","golden_pickaxe","bow_pulling_0","carrot_on_a_stick","leather","saddle","beef","cooked_beef","ender_pearl","blaze_rod","melon_slice","green_dye","lime_dye" },
        {"wooden_axe","stone_axe","iron_axe","diamond_axe","golden_axe","bow_pulling_1","baked_potato","potato","carrot","chicken","cooked_chicken","ghast_tear","gold_nugget","nether_wart","cocoa_beans","yellow_dye" },
        {"wooden_hoe","stone_hoe","iron_hoe","diamond_hoe","golden_hoe","bow_pulling_2","poisonous_potato","minecart","oak_boat","glistering_melon_slice","fermented_spider_eye","spider_eye","potion","potion_overlay","blue_dye","light_blue_dye" },
        {"leather_helmet_overlay","spectral_arrow","iron_horse_armor","diamond_horse_armor","golden_horse_armor","comparator","golden_carrot","chest_minecart","pumpkin_pie","spawn_egg","splash_potion","ender_eye","cauldron","blaze_powder","purple_dye","magenta_dye" },
        {"","tipped_arrow_base","dragon_breath","name_tag","lead","nether_brick","tropical_fish","furnace_minecart","charcoal","spawn_egg_overlay","","experience_bottle","brewing_stand","magma_cream","cyan_dye","orange_dye" },
        {"leather_leggings_overlay","tipped_arrow_head","lingering_potion","barrier","mutton","rabbit","pufferfish","hopper_minecart","hopper","nether_star","emerald","writable_book","written_book","flower_pot","light_gray_dye","bone_meal" },
        {"leather_boots_overlay","beetroot","beetroot_seeds","beetroot_soup","cooked_mutton","cooked_rabbit","salmon","tnt_minecart","armor_stand","firework_rocket","firework_star","firework_star_overlay","quartz","map","item_frame","enchanted_book" },
        {"acacia_door","birch_door","dark_oak_door","jungle_door","spruce_door","rabbit_stew","cooked_salmon","command_block_minecart","acacia_boat","birch_boat","dark_oak_boat","jungle_boat","spruce_boat","prismarine_shard","prismarine_crystals","leather_horse_armor" },
        {"structure_void","","totem_of_undying","shulker_shell","iron_nugget","rabbit_foot","rabbit_hide","","","","","","","","","" },
        {"music_disc_13","music_disc_cat","music_disc_blocks","music_disc_chirp","music_disc_far","music_disc_mall","music_disc_mellohi","music_disc_stal","music_disc_strad","music_disc_ward","music_disc_11","music_disc_wait","cod_bucket","salmon_bucket","pufferfish_bucket","tropical_fish_bucket" },
        {"leather_horse_armor","","","","","","","kelp","dried_kelp","sea_pickle","nautilus_shell","heart_of_the_sea","turtle_helmet","scute","trident","phantom_membrane" }
        };

        string[,] BlockSheetArray =
        {
            {"grass_block_top","stone","dirt","grass_block_side","oak_planks","smooth_stone_slab_side","smooth_stone","bricks","tnt_side","tnt_top","tnt_bottom","cobweb","poppy","dandelion","blue_concrete","oak_sapling" },
            {"cobblestone","bedrock","sand","gravel","oak_log","oak_log_top","iron_block","gold_block","diamond_block","emerald_block","redstone_block","dropper_front","red_mushroom","brown_mushroom","jungle_sapling","red_concrete" },
            {"gold_ore","iron_ore","coal_ore","bookshelf","mossy_cobblestone","obsidian","grass_block_side_overlay","grass","dispenser_front_vertical","beacon","dropper_front_vertical","crafting_table_top","furnace_front","furnace_side","dispenser_front","red_concrete" },
            {"sponge","glass","diamond_ore","redstone_ore","oak_leaves","black_concrete","stone_bricks","dead_bush","fern","daylight_detector_top","daylight_detector_side","crafting_table_side","crafting_table_front","furnace_front_on","furnace_top","spruce_sapling" },
            {"white_wool","spawner","snow","ice","grass_block_snow","cactus_top","cactus_side","cactus_bottom","clay","sugar_cane","jukebox_side","jukebox_top","birch_leaves","mycelium_side","mycelium_top","birch_sapling" },
            {"torch","oak_door_top","iron_door_top","ladder","oak_trapdoor","iron_bars","farmland_wet","farmland","wheat_stage0","wheat_stage1","wheat_stage2","wheat_stage3","wheat_stage4","wheat_stage5","wheat_stage6","wheat_stage7" },
            {"lever","oak_door_bottom","iron_door_bottom","redstone_torch","mossy_stone_bricks","cracked_stone_bricks","pumpkin_top","netherrack","soul_sand","glowstone","piston_top_sticky","piston_top","piston_side","piston_bottom","piston_inner","pumpkin_stem" },
            {"rail_corner","black_wool","gray_wool","redstone_torch_off","spruce_log","birch_log","pumpkin_side","carved_pumpkin","jack_o_lantern","cake_top","cake_side","cake_inner","cake_bottom","red_mushroom_block","brown_mushroom_block","attached_pumpkin_stem" },
            {"rail","red_wool", "pink_wool","repeater","spruce_leaves","spruce_leaves","conduit","turtle_egg","melon_side","melon_top","cauldron_top","cauldron_inner","wet_sponge","mushroom_stem","mushroom_block_inside","vines" },
            {"lapis_block","green_wool","lime_wool","repeater_on","glass_pane_top","debug","debug","turtle_egg_slightly_cracked","turtle_egg_very_cracked","jungle_log","cauldron_side","cauldron_bottom","brewing_stand_base","brewing_stand","end_portal_frame_top","end_portal_frame_side" },
            {"lapis_ore","brown_wool","yellow_wool","powered_rail","redstone_dust_dot","redstone_dust_line0","enchanting_table_top","dragon_egg","cocoa_stage2","cocoa_stage1","cocoa_stage0","emerald_ore","tripwire_hook","tripwire","end_portal_frame_eye","end_stone" },
            {"sandstone_top","blue_wool","light_blue_wool","powered_rail_on","debug","debug","enchanting_table_side","enchanting_table_bottom","glide_blue","item_frame","flower_pot","comparator","comparator_on","activator_rail","activator_rail","nether_quartz_ore" },
            {"sandstone","purple_wool","magenta_wool","detector_rail","jungle_leaves","black_concrete","spruce_planks","jungle_planks","carrots_stage0","carrots_stage1","carrots_stage2","carrots_stage3","slime_block","debug","debug","debug" },
            {"sandstone_bottom","cyan_wool","orange_wool","redstone_lamp","redstone_lamp_on","chiseled_stone_bricks","birch_planks","anvil","chipped_anvil_top","chiseled_quartz_block_top","quartz_pillar_top","quartz_block_side","debug","detector_rail_on","debug","debug" },
            {"nether_bricks","light_gray_wool","nether_wart_stage0","nether_wart_stage1","nether_wart_stage2","chiseled_sandstone","cut_sandstone","anvil_top","damaged_anvil_top","chiseled_quartz_block","quartz_pillar","quartz_block_top","debug","debug","debug","debug" },
            {"destroy_stage_0","destroy_stage_1","destroy_stage_2","destroy_stage_3","destroy_stage_4","destroy_stage_5","destroy_stage_6","destroy_stage_7","destroy_stage_8","destroy_stage_9","hay_block_side","quartz_block_bottom","debug","hay_block_top","debug","debug" },
            {"coal_block","terracotta","note_block","andesite","polished_andesite","diorite","polished_diorite","granite","polished_granite","potatoes_stage0","potatoes_stage1","potatoes_stage2","potatoes_stage3","spruce_log_top","jungle_log_top","birch_log_top" },
            {"black_terracotta","blue_terracotta","brown_terracotta","cyan_terracotta","gray_terracotta","green_terracotta","light_blue_terracotta","lime_terracotta","magenta_terracotta","orange_terracotta","pink_terracotta","purple_terracotta","red_terracotta","light_gray_terracotta","white_terracotta","yellow_terracotta" },
            {"black_stained_glass","blue_stained_glass","brown_stained_glass","cyan_stained_glass","gray_stained_glass","green_stained_glass","light_blue_stained_glass","lime_stained_glass","magenta_stained_glass","orange_stained_glass","pink_stained_glass","purple_stained_glass","red_stained_glass","light_gray_stained_glass","white_stained_glass","yellow_stained_glass" },
            {"black_stained_glass_pane_top","blue_stained_glass_pane_top","brown_stained_glass_pane_top","cyan_stained_glass_pane_top","gray_stained_glass_pane_top","green_stained_glass_pane_top","light_blue_stained_glass_pane_top","lime_stained_glass_pane_top","magenta_stained_glass_pane_top","orange_stained_glass_pane_top","pink_stained_glass_pane_top","purple_stained_glass_pane_top","red_stained_glass_pane_top","light_gray_stained_glass_pane_top","white_stained_glass_pane_top","yellow_stained_glass_pane_top" },
            {"large_fern_top","tall_grass_top","peony_top","rose_bush_top","lilac_top","orange_tulip","sunflower_top","sunflower_front","acacia_log","acacia_log_top","acacia_planks","acacia_leaves","acacia_leaves","prismarine_bricks","red_sand","red_sandstone_top" },
            {"large_fern_bottom","tall_grass_bottom","peony_bottom","rose_bush_bottom","lilac_bottom","pink_tulip","sunflower_bottom","sunflower_back","dark_oak_log","dark_oak_log_top","dark_oak_planks","dark_oak_leaves","dark_oak_leaves","dark_prismarine","red_sandstone_bottom","red_sandstone" },
            {"allium","blue_orchid","azure_bluet","oxeye_daisy","red_tulip","white_tulip","acacia_sapling","dark_oak_sapling","coarse_dirt","podzol_side","podzol_top","spruce_leaves","spruce_leaves","debug","chiseled_red_sandstone","cut_red_sandstone" },
            {"acacia_door_top","birch_door_top","dark_oak_door_top","jungle_door_top","spruce_door_top","chorus_flower","chorus_flower_dead","chorus_plant","end_stone_bricks","grass_path_side","grass_path_top","debug","packed_ice","debug","daylight_detector_inverted_top","iron_trapdoor" },
            {"acacia_door_bottom","birch_door_bottom","dark_oak_door_bottom","jungle_door_bottom","spruce_door_bottom","purpur_block","purpur_pillar","purpur_pillar_top","end_rod","debug","nether_wart_block","red_nether_bricks","frosted_ice_0","frosted_ice_1","frosted_ice_2","frosted_ice_3" },
            {"beetroots_stage0","beetroots_stage1","beetroots_stage2","beetroots_stage3","debug","debug","debug","debug","debug","debug","debug","debug","debug","debug","debug","debug" },
            {"bone_block_side","bone_block_top","melon_stem","attached_melon_stem","observer_front","observer_side","observer_back","observer_back_on","observer_top","glide_yellow","glide_green","structure_block","structure_block_corner","structure_block_data","structure_block_load","structure_block_save" },
            {"black_concrete","blue_concrete","brown_concrete","cyan_concrete","gray_concrete","green_concrete","light_blue_concrete","lime_concrete","magenta_concrete","orange_concrete","pink_concrete","purple_concrete","red_concrete","light_gray_concrete","white_concrete","yellow_concrete" },
            {"black_concrete_powder","blue_concrete_powder","brown_concrete_powder","cyan_concrete_powder","gray_concrete_powder","green_concrete_powder","light_blue_concrete_powder","lime_concrete_powder","magenta_concrete_powder","orange_concrete_powder","pink_concrete_powder","purple_concrete_powder","red_concrete_powder","light_gray_concrete_powder","white_concrete_powder","yellow_concrete_powder" },
            {"black_glazed_terracotta","blue_glazed_terracotta","brown_glazed_terracotta","cyan_glazed_terracotta","gray_glazed_terracotta","green_glazed_terracotta","light_blue_glazed_terracotta","lime_glazed_terracotta","magenta_glazed_terracotta","orange_glazed_terracotta","pink_glazed_terracotta","purple_glazed_terracotta","red_glazed_terracotta","light_gray_glazed_terracotta","white_glazed_terracotta","yellow_glazed_terracotta" },
            {"white_shulker_box","","water_overlay","debug","tube_coral_block","bubble_coral_block","brain_coral_block","fire_coral_block","horn_coral_block","tube_coral","bubble_coral","brain_coral","fire_coral","horn_coral","sea_pickle","blue_ice" },
            {"dried_kelp_top","dried_kelp_side","debug","debug","dead_tube_coral_block","dead_bubble_coral_block","dead_brain_coral_block","dead_fire_coral_block","dead_horn_coral_block","tube_coral_fan","bubble_coral_fan","brain_coral_fan","fire_coral_fan","horn_coral_fan","","" },
            {"debug","debug","debug","debug","debug","debug","debug","debug","debug","dead_tube_coral_fan","dead_bubble_coral_fan","dead_brain_coral_fan","dead_fire_coral_fan","dead_horn_coral_fan","","spruce_trapdoor" },
            {"stripped_oak_log","stripped_oak_log_top","stripped_acacia_log","stripped_acacia_log_top","stripped_birch_log","stripped_birch_log_top","stripped_dark_oak_log","stripped_dark_oak_log_top","stripped_jungle_log","stripped_jungle_log_top","stripped_spruce_log","stripped_spruce_log_top","acacia_trapdoor","birch_trapdoor","dark_oak_trapdoor","jungle_trapdoor" }
        };

        string[,] mobs =
        {
            {"\\alex","\\alex"},
            {"\\steve","\\char"},
            {"\\bat","\\bat"},
            {"\\chicken","\\chicken"},
            {"\\dolphin","\\dolphin"},
            {"\\endermite","\\endermite"},
            {"\\guardian","\\guardian"},
            {"\\guardian_beam","\\guardian_beam"},
            {"\\guardian_elder","\\guardian_elder"},
            {"\\phantom","\\phantom"},
            {"\\spider_eyes","\\spider_eyes"},
            {"\\squid","\\squid"},
            {"\\steve","\\steve"},
            {"\\witch","\\witch"},
            {"\\bear\\polarbear","\\bear\\polarbear"},
            {"\\creeper\\creeper","\\creeper"},
            {"\\ghast\\ghast","\\ghast"},
            {"\\ghast\\ghast_shooting","\\ghast_fire"},
            {"\\enderdragon\\dragon_fireball","\\enderdragon\\dragon_fireball"},
            {"\\enderdragon\\dragon","\\enderdragon\\ender"},
            {"\\end_crystal\\end_crystal_beam","\\enderdragon\\beam"},
            {"\\enderdragon\\dragon_eyes","\\enderdragon\\ender_eyes"},
            {"\\enderman\\enderman_eyes","\\enderman\\enderman_eyes"},
            {"\\enderman\\enderman","\\enderman\\enderman"},
            {"\\fish\\cod","\\fish\\cod"},
            {"\\fish\\pufferfish","\\fish\\pufferfish"},
            {"\\fish\\salmon","\\fish\\salmon"},
            {"\\fish\\tropical_a","\\fish\\tropical_a"},
            {"\\fish\\tropical_a_pattern_1","\\fish\\tropical_a_pattern_1"},
            {"\\fish\\tropical_a_pattern_2","\\fish\\tropical_a_pattern_2"},
            {"\\fish\\tropical_a_pattern_3","\\fish\\tropical_a_pattern_3"},
            {"\\fish\\tropical_a_pattern_4","\\fish\\tropical_a_pattern_4"},
            {"\\fish\\tropical_a_pattern_5","\\fish\\tropical_a_pattern_5"},
            {"\\fish\\tropical_a_pattern_6","\\fish\\tropical_a_pattern_6"},
            {"\\fish\\tropical_b","\\fish\\tropical_b"},
            {"\\fish\\tropical_b_pattern_1","\\fish\\tropical_b_pattern_1"},
            {"\\fish\\tropical_b_pattern_2","\\fish\\tropical_b_pattern_2"},
            {"\\fish\\tropical_b_pattern_3","\\fish\\tropical_b_pattern_3"},
            {"\\fish\\tropical_b_pattern_4","\\fish\\tropical_b_pattern_4"},
            {"\\fish\\tropical_b_pattern_5","\\fish\\tropical_b_pattern_5"},
            {"\\fish\\tropical_b_pattern_6","\\fish\\tropical_b_pattern_6"},
            {"\\horse\\donkey","\\horse\\donkey"},
            {"\\horse\\horse_black","\\horse\\horse_black"},
            {"\\horse\\horse_brown","\\horse\\horse_brown"},
            {"\\horse\\horse_chestnut","\\horse\\horse_chestnut"},
            {"\\horse\\horse_creamy","\\horse\\horse_creamy"},
            {"\\horse\\horse_darkbrown","\\horse\\horse_darkbrown"},
            {"\\horse\\horse_gray","\\horse\\horse_gray"},
            {"\\horse\\horse_markings_blackdots","\\horse\\horse_markings_blackdots"},
            {"\\horse\\horse_markings_white","\\horse\\horse_markings_white"},
            {"\\horse\\horse_markings_whitedots","\\horse\\horse_markings_whitedots"},
            {"\\horse\\horse_markings_whitefield","\\horse\\horse_markings_whitefield"},
            {"\\horse\\horse_skeleton","\\horse\\horse_skeleton"},
            {"\\horse\\horse_white","\\horse\\horse_white"},
            {"\\horse\\horse_zombie","\\horse\\horse_zombie"},
            {"\\horse\\mule","\\horse\\mule"},
            {"\\illager\\evoker","\\illager\\evoker"},
            {"\\illager\\vex","\\illager\\vex"},
            {"\\illager\\vex_charging","\\illager\\vex_charging"},
            {"\\illager\\vindicator","\\illager\\vindicator"},
            {"\\llama\\spit","\\llama\\spit"},
            {"\\parrot\\parrot_blue","\\parrot\\parrot_blue"},
            {"\\parrot\\parrot_green","\\parrot\\parrot_green"},
            {"\\parrot\\parrot_grey","\\parrot\\parrot_grey"},
            {"\\parrot\\parrot_red_blue","\\parrot\\parrot_red_blue"},
            {"\\parrot\\parrot_yellow_blue","\\parrot\\parrot_yellow_blue"},
            {"\\rabbit\\black","\\rabbit\\black"},
            {"\\rabbit\\brown","\\rabbit\\brown"},
            {"\\rabbit\\caerbannog","\\rabbit\\caerbannog"},
            {"\\rabbit\\gold","\\rabbit\\gold"},
            {"\\rabbit\\salt","\\rabbit\\salt"},
            {"\\rabbit\\toast","\\rabbit\\toast"},
            {"\\rabbit\\white","\\rabbit\\white"},
            {"\\rabbit\\white_splotched","\\rabbit\\white_splotched"},
            {"\\shulker\\spark","\\shulker\\spark"},
            {"\\skeleton\\stray","\\skeleton\\stray"},
            {"\\skeleton\\skeleton","\\skeleton"},
            {"\\skeleton\\wither_skeleton","\\skeleton_wither"},
            {"\\skeleton\\stray_overlay","\\skeleton\\stray_overlay"},
            {"\\slime\\slime","\\slime"},
            {"\\villager\\villager","\\villager\\villager"},
            {"\\wither\\wither","\\wither\\wither"},
            {"\\wither\\wither_armor","\\wither\\wither_armor"},
            {"\\wither\\wither_invulnerable","\\wither\\wither_invulnerable"},
            {"\\zombie\\drowned","\\zombie\\drowned"},
            {"\\zombie\\husk","\\zombie\\husk"},
            {"\\zombie_villager\\zombie_villager","\\zombie_villager\\zombie_villager"}
        };

        string[,] painting = 
        {
             {"alban","0","2"},
             {"aztec","0","1"},
             {"aztec2","0","3"},
             {"kebab","0","0"},
             {"alban","1","2"},
             {"aztec","1","1"},
             {"aztec2","1","3"},
             {"kebab","1","0"},
             {"bomb","0","4"},
             {"courbet","2","2"},
             {"creebet","2","8"},
             {"plant","0","5"},
             {"sea","2","4"},
             {"sunset","2","6"},
             {"wasteland","0","6"},
             {"burning_skull","12","8"},
             {"bust","8","2"},
             {"donkey_kong","7","12"},
             {"fighters","6","0"},
             {"graham","4","1"},
             {"match","8","0"},
             {"pigscene","12","4"},
             {"pointer","12","0"},
             {"pool","2","0"},
             {"skeleton","4","12"},
             {"skull_and_roses","8","8"},
             {"stage","8","4"},
             {"void","8","6"},
             {"wanderer","4","0"},

             {"back","0","15"},
             {"back","1","15"},
             {"back","2","15"},
             {"back","3","15"},
             {"back","0","14"},
             {"back","1","14"},
             {"back","2","14"},
             {"back","3","14"},
             {"back","0","13"},
             {"back","1","13"},
             {"back","2","13"},
             {"back","3","13"},
             {"back","0","12"},
             {"back","1","12"},
             {"back","2","12"},
             {"back","3","12"}
        };

        string[,] armour = 
            {
                { "\\armor\\chainmail_layer_1", "\\chain_1"},
                { "\\armor\\chainmail_layer_2", "\\chain_2"},
                { "\\armor\\leather_layer_1", "\\cloth_1"},
                { "\\armor\\leather_layer_1_overlay", "\\cloth_1_boverlay"},
                { "\\armor\\leather_layer_2", "\\cloth_2"},
                { "\\armor\\leather_layer_2_overlay", "\\cloth_2_b"},
                { "\\armor\\diamond_layer_1", "\\diamond_1"},
                { "\\armor\\diamond_layer_2", "\\diamond_2"},
                { "\\armor\\gold_layer_1", "\\gold_1"},
                { "\\armor\\gold_layer_2", "\\gold_2"},
                { "\\armor\\iron_layer_1", "\\iron_1"},
                { "\\armor\\iron_layer_2", "\\iron_2"},
                { "\\armor\\turtle_layer_1", "\\turtle_1"}
            };


        //options
        bool blocks = true;

        bool items = true;

        bool gui = false;

        bool entities = true;

        bool sun = true;

        bool rain = true;

        bool paintings = true;

        bool armor = true;

        #endregion

        #region button actions

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = fbd.SelectedPath;
                    findworking(fbd.SelectedPath);
                }
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = Path.GetDirectoryName(sfd.FileName);
                }
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(blocks)
                fromfolderToSheet(workingDir + "\\block");
            if(items)
                fromfolderToItemSheet(workingDir + "\\item");
            if (gui)
                injectGUIs(workingDir + "\\gui", textBox3.Text);
            if (entities)
                MoveEntities(workingDir + "\\entity");
            if (sun)
                movesun(workingDir + "\\environment");
            if (rain)
                moverain(workingDir + "\\environment");
            if (paintings)
                MakePaintingSheet(workingDir + "\\painting");
            if (armor)
                MoveArmor(workingDir + "\\models");
            Console.WriteLine("Process Complete!");
            MessageBox.Show("Process Complete!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "FUI Files | SkinPS3.fui; SkinWiiU.fui; SkinXbox360.fui";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = opf.FileName;
            }
        }

        #endregion

        #region checkboxes

        //checks terrain bool
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                blocks = true;
            }
            else
            {
                blocks = false;
            }
        }

        //checks item bool
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                items = true;
            }
            else
            {
                items = false;
            }
        }

        //checks fui bool
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                textBox3.Enabled = true;
                button4.Enabled = true;
                gui = true;
            }
            else
            {
                textBox3.Enabled = false;
                button4.Enabled = false;
                gui = false;
            }
        }

        //checks mobs bool
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
                entities = true;
            else
                entities = false;
        }

        //checks sun/moon bool
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
                sun = true;
            else
                sun = false;
        }

        //checks weather bool
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
                rain = true;
            else
                rain = false;
        }

        //checks painting bool
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
                paintings = true;
            else
                paintings = false;
        }

        //checks armor bool
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
                armor = true;
            else
                armor = false;
        }

        #endregion

        #region Create textures

        //blocks
        private Bitmap fromfolderToSheet(string input)
        {
            Directory.CreateDirectory(textBox2.Text + "\\res");
            string[] files = Directory.GetFiles(input);
            Bitmap sheet = new Bitmap(Bitmap.FromFile(files[0]).Width * 16, Bitmap.FromFile(files[0]).Height * 34);
            Graphics g = Graphics.FromImage(sheet);
            g.Clear(Color.Transparent);
            int h = 0;
            int w = 0;
            foreach (string file in files)
            {
                try
                {
                    Bitmap bmp = new Bitmap(Bitmap.FromFile(files[0]));
                    try
                    {
                        Image img = Image.FromFile(input + "\\" + BlockSheetArray[h, w] + ".png");
                        Image img2 = (Image)(new Bitmap(img, new Size(bmp.Width, bmp.Height)));


                        //g.DrawImage(Image.FromFile(input + "\\" + BlockSheetArray[h, w] + ".png"), new Point(w * bmp.Width, h * bmp.Width));
                        g.DrawImage(img2, new Point(w * bmp.Width, h * bmp.Width));
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("DRAW - " + BlockSheetArray[h, w] + " - AT - BlockSheetArray[" + h.ToString() + ", " + w.ToString() + "]");
                    }
                    catch
                    {
                        Image img = Image.FromFile(Environment.CurrentDirectory + "\\data\\block" + "\\" + BlockSheetArray[h, w] + ".png");
                        Image img2 = (Image)(new Bitmap(img, new Size(bmp.Width, bmp.Height)));


                        //g.DrawImage(Image.FromFile(input + "\\" + BlockSheetArray[h, w] + ".png"), new Point(w * bmp.Width, h * bmp.Width));
                        g.DrawImage(img2, new Point(w * bmp.Width, h * bmp.Width));
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("DRAW - " + BlockSheetArray[h, w] + " - AT - BlockSheetArray[" + h.ToString() + ", " + w.ToString() + "]");
                    }
                }
                catch (Exception ec)
                { 
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR DRAWING - AT - BlockSheetArray[" + h.ToString() + ", " + w.ToString() + "] - DRAWING BLANK");
                    Console.WriteLine(ec.Message);
                }
                w++;
                if (w >= 16)
                {
                    h++;
                    w = 0;
                }
                if (h >= 34)
                    break;

            }
            g.Dispose();
            sheet.Save(textBox2.Text + "\\res\\terrain.png", System.Drawing.Imaging.ImageFormat.Png);
            if (TxtFiles.Checked)
                File.Create(textBox2.Text + "\\res\\terrain.png.txt");
            makemipmap();
            return sheet;
        }

        public void makemipmap()
        {
            Bitmap src = new Bitmap(Bitmap.FromFile(textBox2.Text + "\\res\\terrain.png"));
            Bitmap mm1 = ResizeBitmap(src, src.Width/2, src.Height/2);
            Bitmap mm2 = ResizeBitmap(src, src.Width/4, src.Height/4);


            mm1.Save(textBox2.Text + "\\res\\terrainMipMapLevel2.png", System.Drawing.Imaging.ImageFormat.Png);
            mm2.Save(textBox2.Text + "\\res\\terrainMipMapLevel3.png", System.Drawing.Imaging.ImageFormat.Png);


            if (TxtFiles.Checked)
            {
                File.Create(textBox2.Text + "\\res\\terrainMipMapLevel2.png.txt");
                File.Create(textBox2.Text + "\\res\\terrainMipMapLevel3.png.txt");
            }
        }

        public Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }

            return result;
        }

        //items
        private Bitmap fromfolderToItemSheet(string input)
        {
            try
            {
                Directory.CreateDirectory(textBox2.Text + "\\res");
                string[] files = Directory.GetFiles(input);
                Bitmap sheet = new Bitmap(Bitmap.FromFile(files[0]).Width * 16, Bitmap.FromFile(files[0]).Height * 17);
                Graphics g = Graphics.FromImage(sheet);
                g.Clear(Color.Transparent);
                int h = 0;
                int w = 0;
                foreach (string file in files)
                {
                    try
                    {
                        Bitmap bmp = new Bitmap(Bitmap.FromFile(files[0]));
                        try
                        {
                            Image img = Image.FromFile(input + "\\" + ItemSheetArray[h, w] + ".png");
                            Image img2 = (Image)(new Bitmap(img, new Size(bmp.Width, bmp.Height)));


                            //g.DrawImageUnscaled(Image.FromFile(input + "\\" + ItemSheetArray[h, w] + ".png"), new Point(w * Bitmap.FromFile(files[0]).Width, h * Bitmap.FromFile(files[0]).Width));
                            g.DrawImage(img2, new Point(w * bmp.Width, h * bmp.Width));
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("DRAW - " + ItemSheetArray[h, w] + " - AT - ItemSheetArray[" + h.ToString() + ", " + w.ToString() + "]");
                        }
                        catch
                        {
                            Image img = Image.FromFile(Environment.CurrentDirectory + "\\data\\item" + "\\" + ItemSheetArray[h, w] + ".png");
                            Image img2 = (Image)(new Bitmap(img, new Size(bmp.Width, bmp.Height)));


                            //g.DrawImageUnscaled(Image.FromFile(input + "\\" + ItemSheetArray[h, w] + ".png"), new Point(w * Bitmap.FromFile(files[0]).Width, h * Bitmap.FromFile(files[0]).Width));
                            g.DrawImage(img2, new Point(w * bmp.Width, h * bmp.Width));
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("DRAW - " + ItemSheetArray[h, w] + " - AT - ItemSheetArray[" + h.ToString() + ", " + w.ToString() + "]");
                        }
                    }
                    catch (Exception ec)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ERROR DRAWING - AT - ItemSheetArray[" + h.ToString() + ", " + w.ToString() + "] - DRAWING BLANK");
                        Console.WriteLine(ec.Message);
                    }
                    w++;
                    if (w >= 16)
                    {
                        h++;
                        w = 0;
                    }
                    if (h >= 17)
                        break;

                }
                g.Dispose();
                sheet.Save(textBox2.Text + "\\res\\items.png", System.Drawing.Imaging.ImageFormat.Png);
                if (TxtFiles.Checked)
                    File.Create(textBox2.Text + "\\res\\items.png.txt");
                return sheet;
            }
            catch
            {
                Properties.Resources.pack.Save(Environment.CurrentDirectory + "\\pack.png");
                Bitmap bitmap = new Bitmap(Image.FromFile(Environment.CurrentDirectory + "\\pack.png"));
                return bitmap;
            }
        }

        //mobs
        public void MoveEntities(string input)
        {
            Directory.CreateDirectory(textBox2.Text + "\\res\\mob");
            int num = 0;
            while (num <= 133)
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(textBox2.Text + "\\res\\mob" + mobs[num, 1] + ".png"));
                    File.Copy(input + mobs[num, 0]+".png", textBox2.Text + "\\res\\mob" + mobs[num, 1]+".png", true);
                    if (TxtFiles.Checked)
                        File.Create(textBox2.Text + "\\res\\mob" + mobs[num, 1] + ".png.txt");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("COPIED TEXTURE - AT - mobs["+num.ToString()+","+"0]");
                }
                catch (Exception ec)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR COPYING TEXTURE - AT - mobs[" + num.ToString() + "," + "0]");
                    Console.WriteLine(ec.Message);
                }
                num++;
            }

        }

        //sun & moon
        public void movesun(string input)
        {
            Directory.CreateDirectory(textBox2.Text + "\\res\\terrain");
            try
            {
            File.Copy(input + "\\sun.png", textBox2.Text +"\\res\\terrain\\sun.png", true);
            File.Copy(input + "\\moon_phases.png", textBox2.Text + "\\res\\terrain\\moon_phases.png", true);


                if (TxtFiles.Checked)
                {
                    File.Create(textBox2.Text + "\\res\\terrain\\sun.png.txt");
                    File.Create(textBox2.Text + "\\res\\terrain\\moon_phases.png.txt");
                }
            }
            catch (Exception ec)
            {

                Console.WriteLine("[!]something went wrong! see the error printed below!");
                Console.WriteLine(ec.Message);
            }
        }

        //rain & snow
        public void moverain(string input)
        {
            Directory.CreateDirectory(textBox2.Text + "\\res\\environment");
            try
            {
                File.Copy(input + "\\rain.png", textBox2.Text + "\\res\\environment\\rain.png", true);
                File.Copy(input + "\\snow.png", textBox2.Text + "\\res\\environment\\snow.png", true);
                File.Copy(input + "\\clouds.png", textBox2.Text + "\\res\\environment\\clouds.png", true);


                if (TxtFiles.Checked)
                {
                    File.Create(textBox2.Text + "\\res\\environment\\rain.png.txt");
                    File.Create(textBox2.Text + "\\res\\environment\\snow.png.txt");
                    File.Create(textBox2.Text + "\\res\\environment\\clouds.png.txt");
                }
            }
            catch (Exception ec)
            {

                Console.WriteLine("[!]something went wrong! see the error printed below!");
                Console.WriteLine(ec.Message);
            }

        }

        //paintings
        public void MakePaintingSheet(string input)
        {
            try
            {
                Directory.CreateDirectory(textBox2.Text + "\\res\\art");
                string[] files = Directory.GetFiles(input);
                Bitmap sheet = new Bitmap(Bitmap.FromFile(files[0]).Width * 16, Bitmap.FromFile(files[0]).Height * 16);
                Graphics g = Graphics.FromImage(sheet);
                g.Clear(Color.Transparent);
                int w = Bitmap.FromFile(files[0]).Width;
                int h = Bitmap.FromFile(files[0]).Height;
                int i = 0;
                while (i < 45)
                {
                    try
                    {



                        Bitmap bmp = new Bitmap(Bitmap.FromFile(input + "\\" + painting[i, 0] + ".png"));
                        Point location = new Point((int.Parse(painting[i, 2])) * w, (int.Parse(painting[i, 1])) * h);
                        Image img = Image.FromFile(input + "\\" + painting[i, 0] + ".png");
                        Image img2 = (Image)(new Bitmap(img, new Size(bmp.Width, bmp.Height)));


                        //g.DrawImageUnscaled(Image.FromFile(input + "\\" + painting[i, 0] + ".png"), new Point(int.Parse(painting[i, 2]) * Bitmap.FromFile(files[0]).Width, int.Parse(painting[i, 1]) * Bitmap.FromFile(files[0]).Width));
                        g.DrawImage(img2, location);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("DRAW - " + painting[i, 0] + " - AT - Painting[" + i.ToString() + ", 0]");
                        Console.WriteLine("-- " + input + "\\painting\\" + painting[i, 0] + ".png");
                    }
                    catch (Exception ec)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ERROR DRAWING - AT - Painting[" + i.ToString() + ", 0] - DRAWING BLANK");
                        Console.WriteLine("-- " + input + "\\painting\\" + painting[i, 0] + ".png");
                        Console.WriteLine(ec.Message);
                    }
                    i++;

                }
                try
                {
                    g.Dispose();
                    sheet.Save(textBox2.Text + "\\res\\art\\kz.png", System.Drawing.Imaging.ImageFormat.Png);
                    if (TxtFiles.Checked)
                        File.Create(textBox2.Text + "\\res\\art\\kz.png.txt");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("SAVE - " + textBox2.Text + "\\res\\art\\kz.png");
                }
                catch { }
            }
            catch (Exception ec)
            {

                Console.WriteLine("[!]something went wrong! see the error printed below!");
                Console.WriteLine(ec.Message);
            }
        }

        //GUI loading and packing
        public void injectGUIs(string input, string FUI)
        {
                
        }

        //armor layers
        public void MoveArmor(string input)
        {
            Directory.CreateDirectory(textBox2.Text + "\\res\\armor");
            int num = 0;
            while (num <= 14)
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(textBox2.Text + "\\res\\armor" + armour[num, 1] + ".png"));
                    File.Copy(input + armour[num, 0] + ".png", textBox2.Text + "\\res\\armor" + armour[num, 1] + ".png", true);
                    if(TxtFiles.Checked)
                    File.Create(textBox2.Text + "\\res\\armor" + armour[num, 1] + ".png.txt");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("COPIED TEXTURE - AT - armor[" + num.ToString() + "," + "0]");
                }
                catch (Exception ec)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR COPYING TEXTURE - AT - armor[" + num.ToString() + "," + "0]");
                    Console.WriteLine(ec.Message);
                }
                num++;
            }

        }


        #endregion

        #region startup actions

        private void Form1_Load(object sender, EventArgs e)
        {
            //displays version in top right
            label4.Text = CurrentVersion;


            //sets up Environment and folders
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\OutputTextures");
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\InputTextures");
            textBox2.Text = Environment.CurrentDirectory + "\\OutputTextures";
            textBox1.Text = Environment.CurrentDirectory + "\\InputTextures";
            if(!Directory.Exists(Environment.CurrentDirectory + "\\data"))
            {
                Console.WriteLine("==Extracting backup texture data...");
                ZipFile.ExtractToDirectory(Environment.CurrentDirectory + "\\TextureData.zip", Environment.CurrentDirectory);
                Console.WriteLine("==Extracted backup texture data!");
            }

            //Update
            WebClient wc = new WebClient();
            try
            {
                update(BaseURL + updatePath);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[!]Could not contact main server, using backup..");
                Console.ForegroundColor = ConsoleColor.Green;
                try
                {
                    update(BackURL + updatePath);
                }
                catch (Exception ec)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!]something went wrong! see the error printed below!");
                    Console.WriteLine(ec.Message);
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            }

        }

        public void update(string url)
        {
            WebClient wc = new WebClient();
            string updatedversion = wc.DownloadString(url);
            Console.WriteLine(updatedversion);
            Console.ForegroundColor = ConsoleColor.Green;
            if (float.Parse(updatedversion) > float.Parse(CurrentVersion))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[!]Update avaliable: version " + updatedversion);
                Console.ForegroundColor = ConsoleColor.Green;
                if (MessageBox.Show("Update!", "An update is avaliable, do you want to update?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Console.WriteLine("Updating Software... Exiting");
                    System.Diagnostics.Process.Start("TextureToolUpdater.exe");
                    Application.Exit();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!]Update Cancelled");
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            }
            else
            {
                Console.WriteLine("[=^w^=]Software up to date!");
            }
        }

        #endregion

        #region misc actions

        public void findworking(string path)
        {
            foreach(string dir in Directory.GetDirectories(path))
            {
                switch (dir.Split('\\')[dir.Split('\\').Length - 1])
                {
                    case ("assets"):
                        string path1 = dir.Split('\\')[dir.Split('\\').Length - 1];
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("[!] 1 step");
                        Console.ForegroundColor = ConsoleColor.Green;

                        foreach (string dir1 in Directory.GetDirectories(path + "\\" + path1))
                        {
                            switch (dir1.Split('\\')[dir1.Split('\\').Length - 1])
                            {
                                case ("minecraft"):
                                    string pathx = dir1;
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("[!] 2 step");
                                    Console.ForegroundColor = ConsoleColor.Green;

                                    foreach (string dir2 in Directory.GetDirectories(pathx))
                                    {
                                        switch (dir2.Split('\\')[dir2.Split('\\').Length - 1])
                                        {
                                            case ("textures"):
                                                workingDir = dir2;
                                                Console.ForegroundColor = ConsoleColor.Yellow;
                                                Console.WriteLine("[!] 3 step");

                                                Console.WriteLine("[!] WorkingDirectory is now: " + workingDir);
                                                Console.ForegroundColor = ConsoleColor.Green;

                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case ("minecraft"):
                        string path2 = dir;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("[!] 1 step");
                        Console.ForegroundColor = ConsoleColor.Green;

                        foreach (string dir2 in Directory.GetDirectories(path2))
                        {
                            switch (dir2.Split('\\')[dir2.Split('\\').Length - 1])
                            {
                                case ("block"):
                                    workingDir = dir2;
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("[!] 2 step");

                                    Console.WriteLine("[!] WorkingDirectory is now: " + workingDir);
                                    Console.ForegroundColor = ConsoleColor.Green;

                                    break;
                            }
                        }
                        break;
                    case ("textures"):
                        workingDir = dir;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("[!] 1 step");

                        Console.WriteLine("[!] WorkingDirectory is now: " + workingDir);
                        Console.ForegroundColor = ConsoleColor.Green;

                        break;
                }
            }
        }

        #endregion

    }
}
