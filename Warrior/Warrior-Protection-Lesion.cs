// winifix@gmail.com
// ReSharper disable UnusedMember.Global

using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using PixelMagic.Helpers;
using PixelMagic.GUI;

namespace PixelMagic.Rotation
{
    public class ProtectionLesion : CombatRoutine
    {	

	    //will interrupt anything it can
		private CheckBox generalint;
		//will check a list of spells and interrupt ones deemed important by wowhead guides
        private CheckBox mplusint;
		//will automatically use def cooldowns
		private CheckBox CDint;
		//weaves spell reflect into pummel. it wont use it on cooldown.
		private CheckBox SRint;
		//Tick for ImpendingVictory
		private CheckBox IVint;
		//Tick for Indomitable
		private CheckBox IDint;
		//Tick for Auto Battlecry
		private CheckBox BCint;
		//Tick for HP Pots
		private CheckBox HPint;
		//Tick for M+ booming/Angermanagement
		private CheckBox AMint;
		
		
        public override string Name => "Protection Warrior";

        public override string Class => "Warrior";
		
		private static bool generalInterrupts
        {
            get
            {
                var generalInterrupts = ConfigFile.ReadValue("ProtectionLesion", "generalInterrupts").Trim();

                return generalInterrupts != "" && Convert.ToBoolean(generalInterrupts);
            }
            set { ConfigFile.WriteValue("ProtectionLesion", "generalInterrupts", value.ToString()); }
        }

        private static bool mythicplusinterrupts
        {
            get
            {
                var mythicplusinterrupts = ConfigFile.ReadValue("ProtectionLesion", "mythicplusinterrupts").Trim();

                return mythicplusinterrupts != "" && Convert.ToBoolean(mythicplusinterrupts);
            }
            set { ConfigFile.WriteValue("ProtectionLesion", "mythicplusinterrupts", value.ToString()); }
        }
		
		private static bool defcooldowns
        {
            get
            {
                var defcooldowns = ConfigFile.ReadValue("ProtectionLesion", "defcooldowns").Trim();

                return defcooldowns != "" && Convert.ToBoolean(defcooldowns);
            }
            set { ConfigFile.WriteValue("ProtectionLesion", "defcooldowns", value.ToString()); }
        }
		
		private static bool spellref
        {
            get
            {
                var spellref = ConfigFile.ReadValue("ProtectionLesion", "spellref").Trim();

                return spellref != "" && Convert.ToBoolean(spellref);
            }
            set { ConfigFile.WriteValue("ProtectionLesion", "spellref", value.ToString()); }
        }
		
		private static bool ImpendingVic
        {
            get
            {
                var ImpendingVic = ConfigFile.ReadValue("ProtectionLesion", "ImpendingVic").Trim();

                return ImpendingVic != "" && Convert.ToBoolean(ImpendingVic);
            }
            set { ConfigFile.WriteValue("ProtectionLesion", "ImpendingVic", value.ToString()); }
        }
		
		private static bool Indomitable
        {
            get
            {
                var Indomitable = ConfigFile.ReadValue("ProtectionLesion", "Indomitable").Trim();

                return Indomitable != "" && Convert.ToBoolean(Indomitable);
            }
            set { ConfigFile.WriteValue("ProtectionLesion", "Indomitable", value.ToString()); }
        }
		
		private static bool BattleC
        {
            get
            {
                var BattleC = ConfigFile.ReadValue("ProtectionLesion", "BattleC").Trim();

                return BattleC != "" && Convert.ToBoolean(BattleC);
            }
            set { ConfigFile.WriteValue("ProtectionLesion", "BattleC", value.ToString()); }
        }
		
		private static bool Pots
        {
            get
            {
                var Pots = ConfigFile.ReadValue("ProtectionLesion", "Pots").Trim();

                return Pots != "" && Convert.ToBoolean(Pots);
            }
            set { ConfigFile.WriteValue("ProtectionLesion", "Pots", value.ToString()); }
        }
		private static bool AngerM
        {
            get
            {
                var AngerM = ConfigFile.ReadValue("ProtectionLesion", "AngerM").Trim();

                return AngerM != "" && Convert.ToBoolean(AngerM);
            }
            set { ConfigFile.WriteValue("ProtectionLesion", "AngerM", value.ToString()); }
        }
	
		private readonly Stopwatch swingwatch = new Stopwatch();

        public override Form SettingsForm { get; set; }

        public override void Initialize()
        {
		
            Log.Write("Welcome to Protection Warrior", Color.Red);
            Log.Write("Suggested build: 1213112", Color.Red);
			Log.Write("3.21", Color.Red);
			Log.Write("Last Edited by Lesion 25/01/17 - added rage limit for impending victory ", Color.Blue);
            WoW.Speak("Welcome to PixelMagic Protection Warrior by Lesion");
			
			SettingsForm = new Form {Text = "Settings", StartPosition = FormStartPosition.CenterScreen, Width = 160, Height = 250, ShowIcon = false};

            //var picBox = new PictureBox {Left = 0, Top = 0, Width = 800, Height = 100, Image = TopLogo};
            //SettingsForm.Controls.Add(picBox);

            var lblGeneralInterruptsText = new Label //12; 114 LEFT is first value, Top is second.
            {
                Text = "Interrupt all",
                Size = new Size(81, 13), //81; 13
                Left = 12,
                Top = 14
            };
            SettingsForm.Controls.Add(lblGeneralInterruptsText); //113; 114 

            generalint = new CheckBox {Checked = generalInterrupts, TabIndex = 2, Size = new Size(15, 14), Left = 115, Top = 14};
            SettingsForm.Controls.Add(generalint);

            var lblMythicPlusText = new Label //12; 129 is first value, Top is second.
            {
                Text = "M+ Interrupts",
                Size = new Size(95, 13), //95; 13
                Left = 12,
                Top = 29
            };
            SettingsForm.Controls.Add(lblMythicPlusText); //113; 114 

            mplusint = new CheckBox {Checked = mythicplusinterrupts, TabIndex = 4, Size = new Size(15, 14), Left = 115, Top = 29};
            SettingsForm.Controls.Add(mplusint);
			
			var lbldefcooldownsText = new Label //12; 129 is first value, Top is second.
            {
                Text = "Def Cooldowns used automatically SW/LS",
                Size = new Size(95, 13), //95; 13
                Left = 12,
                Top = 44
            };
            SettingsForm.Controls.Add(lbldefcooldownsText); //113; 114 

            CDint = new CheckBox {Checked = defcooldowns, TabIndex = 5, Size = new Size(15, 14), Left = 115, Top = 44};
            SettingsForm.Controls.Add(CDint);
			
			var lblspellrefText = new Label //12; 129 is first value, Top is second.
            {
                Text = "Spell Reflect",
                Size = new Size(95, 13), //95; 13
                Left = 12,
                Top = 59
            };
            SettingsForm.Controls.Add(lblspellrefText); //113; 114 

            SRint = new CheckBox {Checked = spellref, TabIndex = 6, Size = new Size(15, 14), Left = 115, Top = 59};
            SettingsForm.Controls.Add(SRint);
			
			var lblImpendingVicText = new Label //12; 129 is first value, Top is second.
            {
                Text = "Impending Victory",
                Size = new Size(95, 13), //95; 13
                Left = 12,
                Top = 74
            };
            SettingsForm.Controls.Add(lblImpendingVicText); //113; 114 

            IVint = new CheckBox {Checked = ImpendingVic, TabIndex = 7, Size = new Size(15, 14), Left = 115, Top = 74};
            SettingsForm.Controls.Add(IVint);
			
			var lblIndomitableText = new Label //12; 129 is first value, Top is second.
            {
                Text = "Indomitable",
                Size = new Size(95, 13), //95; 13
                Left = 12,
                Top = 89
            };
            SettingsForm.Controls.Add(lblIndomitableText); //113; 114 

            IDint = new CheckBox {Checked = Indomitable, TabIndex = 7, Size = new Size(15, 14), Left = 115, Top = 89};
            SettingsForm.Controls.Add(IDint);
			
			var lblBattleCText = new Label //12; 129 is first value, Top is second.
            {
                Text = "Battle Cry",
                Size = new Size(95, 13), //95; 13
                Left = 12,
                Top = 104
            };
            SettingsForm.Controls.Add(lblBattleCText); //113; 114 

            BCint = new CheckBox {Checked = BattleC, TabIndex = 7, Size = new Size(15, 14), Left = 115, Top = 104};
            SettingsForm.Controls.Add(BCint);
			
			var lblHPText = new Label //12; 129 is first value, Top is second.
            {
                Text = "Use HP Pot",
                Size = new Size(95, 13), //95; 13
                Left = 12,
                Top = 119
            };
            SettingsForm.Controls.Add(lblHPText); //113; 114 

            HPint = new CheckBox {Checked = BattleC, TabIndex = 7, Size = new Size(15, 14), Left = 115, Top = 119};
            SettingsForm.Controls.Add(HPint);
			
			var lblAMText = new Label //12; 129 is first value, Top is second.
            {
                Text = "M+ AM/BV",
                Size = new Size(95, 13), //95; 13
                Left = 12,
                Top = 134
            };
            SettingsForm.Controls.Add(lblAMText); //113; 114 

            AMint = new CheckBox {Checked = BattleC, TabIndex = 7, Size = new Size(15, 14), Left = 115, Top = 134};
            SettingsForm.Controls.Add(AMint);
			 
            var cmdSave = new Button {Text = "Save", Width = 65, Height = 25, Left = 15, Top = 150, Size = new Size(120, 48)};

            generalint.Checked = generalInterrupts;
            mplusint.Checked = mythicplusinterrupts;
			CDint.Checked = defcooldowns;
			SRint.Checked = spellref;
			IVint.Checked = ImpendingVic;
			IDint.Checked = Indomitable;
			BCint.Checked = BattleC;
			HPint.Checked = Pots;
			AMint.Checked = AngerM;
            

            cmdSave.Click += CmdSave_Click;
            generalint.CheckedChanged += GI_Click;
            mplusint.CheckedChanged += MP_Click;
            CDint.CheckedChanged += CD_Click;
			SRint.CheckedChanged += SR_Click;
			IVint.CheckedChanged += IV_Click;
			IDint.CheckedChanged += ID_Click;
			BCint.CheckedChanged += BC_Click;
			HPint.CheckedChanged += HP_Click;
			AMint.CheckedChanged += AM_Click;

            SettingsForm.Controls.Add(cmdSave);
            lblGeneralInterruptsText.BringToFront();
            lblMythicPlusText.BringToFront();
			lbldefcooldownsText.BringToFront();
			lblspellrefText.BringToFront();
			lblImpendingVicText.BringToFront();
			lblIndomitableText.BringToFront();
			lblBattleCText.BringToFront();
			lblHPText.BringToFront();
			lblAMText.BringToFront();
			
			
			
            

            Log.Write("Interupt all = " + generalInterrupts, Color.Red);
            Log.Write("Mythic Plus = " + mythicplusinterrupts, Color.Red);
			Log.Write("Def-cooldowns being used = " + defcooldowns, Color.Red);
			Log.Write("Spell Reflect = " + spellref, Color.Red);
			Log.Write("Using Impending Victory = " + ImpendingVic, Color.Red);
			Log.Write("Using Indomitable Talent = " + Indomitable, Color.Red);
			Log.Write("Auto using Battle Cry = " + BattleC, Color.Red);
			Log.Write("Use HP Pots = " + Pots, Color.Red);
			Log.Write("Use Booming voice & Anger Management for M+/AoE..ish = " + AngerM, Color.Red);
			
        }
		private void CmdSave_Click(object sender, EventArgs e)
        {
            generalInterrupts = generalint.Checked;
            mythicplusinterrupts = mplusint.Checked;
			defcooldowns = CDint.Checked;
			spellref = SRint.Checked;
			ImpendingVic = IVint.Checked;
			Indomitable = IDint.Checked;
			BattleC = BCint.Checked;
			Pots = HPint.Checked;
			AngerM = AMint.Checked;
			
			
            MessageBox.Show("Settings saved", "PixelMagic", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SettingsForm.Close();
        }

        private void GI_Click(object sender, EventArgs e)
        {
            generalInterrupts = generalint.Checked;
        }

        private void MP_Click(object sender, EventArgs e)
        {
            mythicplusinterrupts = mplusint.Checked;
        }
		
		private void CD_Click(object sender, EventArgs e)
        {
            defcooldowns = CDint.Checked;
        }
		
		private void SR_Click(object sender, EventArgs e)
        {
            spellref = SRint.Checked;
        }
		
		private void IV_Click(object sender, EventArgs e)
        {
            ImpendingVic = IVint.Checked;
        }
		
		private void ID_Click(object sender, EventArgs e)
        {
            Indomitable = IDint.Checked;
        }
		private void BC_Click(object sender, EventArgs e)
        {
            BattleC = BCint.Checked;
        }
		private void HP_Click(object sender, EventArgs e)
        {
            Pots = HPint.Checked;
        }
		private void AM_Click(object sender, EventArgs e)
        {
            AngerM = AMint.Checked;
        }
		
		
        public override void Stop()
        {
        }
			
        public override void Pulse()
		{
		
		
			
			if (defcooldowns && WoW.IsInCombat)
			{
			if (WoW.HealthPercent < 35 && WoW.CanCast("Last Stand") && !WoW.IsSpellOnCooldown("Last Stand"))
            {
                WoW.CastSpell("Last Stand");
                return;
            }
            if (WoW.HealthPercent < 20 && WoW.CanCast("Shield Wall") && !WoW.IsSpellOnCooldown("Shield Wall"))
            {
                WoW.CastSpell("Shield Wall");
                return;
            }
			}
			
			if (Pots && WoW.IsInCombat && WoW.HealthPercent < 30)
			{
				if(WoW.ItemCount("Healthstone") >= 1 && !WoW.ItemOnCooldown("Healthstone") &&WoW.ItemCount("HealthPotion") == 0)
				{
					WoW.CastSpell("Healthstone");
					return;
				}
				
				if(WoW.ItemCount("HealthPotion") >= 1 && !WoW.ItemOnCooldown("HealthPotion"))
				{
					WoW.CastSpell("HealthPotion");
					return;
				}
				
				
			}
			
			
			
			
			if (!Indomitable &&WoW.IsInCombat &&WoW.IsSpellInRange("Shield Slam"))
            {
                swingwatch.Start();
            }
			
            if (combatRoutine.Type == RotationType.SingleTarget) // Do Single Target Stuff here
            {
							
				
                if (WoW.HasTarget && !WoW.PlayerIsChanneling &&WoW.IsSpellInRange("Shield Slam"))
                {
					
				if (BattleC && !WoW.IsSpellOnCooldown("Battle Cry"))
						{
						WoW.CastSpell("Battle Cry");
						return;
						}
				if (AngerM && !WoW.IsSpellOnCooldown("Demoralizing Shout") && (DetectKeyPress.GetKeyState(DetectKeyPress.Z) < 0))
				{
					WoW.CastSpell("Demoralizing Shout");
				}
				if (generalInterrupts)
					{
					if (WoW.TargetIsCasting &&WoW.TargetIsCastingAndSpellIsInterruptible)
                        {
						if (!WoW.IsSpellOnCooldown("Pummel")&& WoW.IsSpellInRange("Shield Slam")&&WoW.TargetPercentCast >= 50)
						{
						WoW.CastSpell("Pummel");
						return;
						}
						if (spellref &&!WoW.IsSpellOnCooldown("SpellReflect")&&WoW.TargetIsCasting &&WoW.TargetIsCastingAndSpellIsInterruptible &&WoW.TargetPercentCast >= 80)
						{
						WoW.CastSpell("SpellReflect");
						return;	
						}
						}
							
					}
					
					if (mythicplusinterrupts)
					{
                    	if (WoW.CanCast("Pummel")&&WoW.TargetIsCasting &&WoW.TargetIsCastingAndSpellIsInterruptible)
							{
//int spell list for all important spells in M+                        
if ( WoW.TargetCastingSpellID == 200248
//Court Of Stars Mythic+ Interrupt list
|| WoW.TargetCastingSpellID == 225573
|| WoW.TargetCastingSpellID == 208165
|| WoW.TargetCastingSpellID == 211401
|| WoW.TargetCastingSpellID == 21147
|| WoW.TargetCastingSpellID == 211299
|| WoW.TargetCastingSpellID == 2251
|| WoW.TargetCastingSpellID == 209413
|| WoW.TargetCastingSpellID == 209404
|| WoW.TargetCastingSpellID == 215204
|| WoW.TargetCastingSpellID == 210261
//Darkheart Thicket Mythic+ Interrupt list
|| WoW.TargetCastingSpellID == 200658
|| WoW.TargetCastingSpellID == 200631
|| WoW.TargetCastingSpellID == 204246
|| WoW.TargetCastingSpellID == 2014
//Eye of Azshara Mythic+ Interrupt list
|| WoW.TargetCastingSpellID == 19687
|| WoW.TargetCastingSpellID == 218532
|| WoW.TargetCastingSpellID == 195129
|| WoW.TargetCastingSpellID == 195046
|| WoW.TargetCastingSpellID == 197502
|| WoW.TargetCastingSpellID == 196027
|| WoW.TargetCastingSpellID == 196175
|| WoW.TargetCastingSpellID == 192003
|| WoW.TargetCastingSpellID == 191848
//Halls of Valor Mythic+ Interrupt list
|| WoW.TargetCastingSpellID == 198595
|| WoW.TargetCastingSpellID == 198962
|| WoW.TargetCastingSpellID == 198931
|| WoW.TargetCastingSpellID == 192563
|| WoW.TargetCastingSpellID == 192288
|| WoW.TargetCastingSpellID == 199726
//Maw of Souls Mythic+ Interrupt list
|| WoW.TargetCastingSpellID == 198495
|| WoW.TargetCastingSpellID == 195293
|| WoW.TargetCastingSpellID == 199589
|| WoW.TargetCastingSpellID == 194266
|| WoW.TargetCastingSpellID == 198405
|| WoW.TargetCastingSpellID == 199514
|| WoW.TargetCastingSpellID == 194657
//Neltharions Lair Mythic+ Interrupt list
|| WoW.TargetCastingSpellID == 193585
|| WoW.TargetCastingSpellID == 202181
//The Arcway Mythic+ Interrupt list
|| WoW.TargetCastingSpellID == 226269
|| WoW.TargetCastingSpellID == 211007 
|| WoW.TargetCastingSpellID == 211757 
|| WoW.TargetCastingSpellID == 226285 
|| WoW.TargetCastingSpellID == 226206 
|| WoW.TargetCastingSpellID == 211115 
|| WoW.TargetCastingSpellID == 196392 
// Advisor Vandros (Interrupt manually) Spell,203176,Accelerating Blast
|| WoW.TargetCastingSpellID == 203957 
//Vault of the Wardens Mythic+ Interrupt list
|| WoW.TargetCastingSpellID == 193069
|| WoW.TargetCastingSpellID == 191823
|| WoW.TargetCastingSpellID == 202661
|| WoW.TargetCastingSpellID == 201488
|| WoW.TargetCastingSpellID == 195332
//Raid Interrupts
|| WoW.TargetCastingSpellID == 209485
|| WoW.TargetCastingSpellID == 209410
|| WoW.TargetCastingSpellID == 211470
|| WoW.TargetCastingSpellID == 225100
|| WoW.TargetCastingSpellID == 207980
|| WoW.TargetCastingSpellID == 196870
|| WoW.TargetCastingSpellID == 195284
|| WoW.TargetCastingSpellID == 192005
|| WoW.TargetCastingSpellID == 228255
|| WoW.TargetCastingSpellID == 228239
|| WoW.TargetCastingSpellID == 227917
|| WoW.TargetCastingSpellID == 228625
|| WoW.TargetCastingSpellID == 228606
|| WoW.TargetCastingSpellID == 229714
|| WoW.TargetCastingSpellID == 227592
|| WoW.TargetCastingSpellID == 229083
|| WoW.TargetCastingSpellID == 228025
|| WoW.TargetCastingSpellID == 228019
|| WoW.TargetCastingSpellID == 227987
|| WoW.TargetCastingSpellID == 227420
|| WoW.TargetCastingSpellID == 200905)
								
								{
								if (!WoW.IsSpellOnCooldown("Pummel")&&WoW.TargetPercentCast >= 40)
									{
									WoW.CastSpell("Pummel");
									return;
									}
								if (spellref &&!WoW.IsSpellOnCooldown("SpellReflect")&&WoW.TargetPercentCast >= 80)
									{
									WoW.CastSpell("SpellReflect");
									return;	
									}
									
							//		WoW.CastSpell("Pummel");
							//WoW.CastSpell("SpellReflect");
							//return;
								}
							}
					}
																
						if (WoW.CanCast("Shield Block") &&WoW.HealthPercent <= 90 && WoW.Rage >= 15 && !WoW.IsSpellOnCooldown("Shield Block") &&!WoW.PlayerHasBuff("Shield Block"))
                        {
                            WoW.CastSpell("Shield Block");
                            return;
                        }
						
						if (WoW.CanCast("Shield Block") &&WoW.Rage >=15 && WoW.PlayerBuffTimeRemaining("Shield Block") <= 2)
						{
                            WoW.CastSpell("Shield Block");
                            return;
                        }
						
						
                        // IP Control
						if (WoW.CanCast("Ignore Pain") && (WoW.Rage >= 30) && !WoW.PlayerHasBuff("Vengeance Ignore Pain"))
                        {
                            WoW.CastSpell("Ignore Pain");
                            return;
                        }
                        if (WoW.CanCast("Ignore Pain") && (WoW.Rage >= 13) && WoW.PlayerHasBuff("Vengeance Ignore Pain"))
                        {
                            WoW.CastSpell("Ignore Pain");
						}
						
						// Revenge Control
                        if (!AngerM && WoW.CanCast("Revenge") && WoW.Rage >= 43 && !WoW.PlayerHasBuff("Vengeance Revenge"))
                        {
                            WoW.CastSpell("Revenge");
                        }
						if (AngerM && WoW.CanCast("Revenge") && WoW.Rage >= 43)
                        {
                            WoW.CastSpell("Revenge");
                        }
						if (AngerM && WoW.CanCast("Revenge") && WoW.IsSpellOverlayed("Revenge"))
						{
							WoW.CastSpell("Revenge");
						} 
						if (!AngerM && WoW.CanCast("Revenge") && WoW.IsSpellOverlayed("Revenge") && WoW.PlayerHasBuff("Vengeance Revenge"))
						{
							WoW.CastSpell("Revenge");
						}
						//Re-add the lines below if you are swimmming in rage
						//
						//if (WoW.CanCast("Revenge") && WoW.Rage >= 34 && WoW.PlayerHasBuff("Vengeance Revenge") &&!WoW.IsSpellOnCooldown("Revenge"))
						//{
						//	WoW.CastSpell("Revenge");
						//}
						
						//Rotational shiz
						//It will wait 2.7 seconds for a devastate too proc shield slam. (bastardized swing timer)
						if (!Indomitable &&!WoW.IsSpellOnCooldown("Shield Slam") &&swingwatch.ElapsedMilliseconds > 2700)
					{
						if (!WoW.IsSpellOverlayed("Shield Slam"))
						{
							WoW.CastSpell("Shield Slam");
							swingwatch.Reset();
							swingwatch.Start();
							return;
										
						}		
					}
						
						if (Indomitable &&!WoW.IsSpellOnCooldown("Shield Slam"))
						{
							WoW.CastSpell("Shield Slam");
						}
						
						//if (Indomitable &&!WoW.IsSpellOverlayed("Shield Slam"))
						//{
						//	WoW.CastSpell("Shield Slam");
						//	return;
						//}		
					//will cast SS when proc's
						if (WoW.CanCast("Shield Slam") && WoW.IsSpellOverlayed("Shield Slam"))
                        {
                            WoW.CastSpell("Shield Slam");
							return;
                        }
					
					
						if (Indomitable &&WoW.CanCast("Devastate") && WoW.IsSpellOnCooldown("Shield Slam") && WoW.IsSpellOnCooldown("Thunder Clap"))
                        {
                           WoW.CastSpell("Devastate");
                            return;
                        }
					
                        if (WoW.CanCast("Thunder Clap") && !WoW.IsSpellOnCooldown("Thunder Clap"))
                        {
                            WoW.CastSpell("Thunder Clap");
                            return;
                        }
                        					
							if (ImpendingVic &&WoW.Rage >= 10&&!WoW.IsSpellOnCooldown("Impending Victory") && WoW.HealthPercent <= 80)
							{
							WoW.CastSpell("Impending Victory");
							return;
							}
							if (!ImpendingVic &&WoW.IsSpellOverlayed("Victory Rush") && WoW.HealthPercent <= 70)
							{
							WoW.CastSpell("Victory Rush");
							return;
							}
																					
                      
                        
                    }
					//Artifact / Shockwave Combo.
                    if (WoW.CanCast("Neltharion's Fury") && WoW.TargetHasDebuff("ShockWavestun") &&WoW.IsSpellOnCooldown("Neltharion's Fury"))
                    {
                        WoW.CastSpell("Neltharion's Fury");
                        return;
                    }
                
            }
            if (combatRoutine.Type == RotationType.AOE)
            {
                // Do AOE Stuff here
            }
        }
    }
}

/*
[AddonDetails.db]
AddonAuthor=Lesion
AddonName=badgggggggggerui
WoWVersion=Legion - 70100
[SpellBook.db]
Spell,23922,Shield Slam,D1
Spell,20243,Devastate,D2
Spell,6572,Revenge,D3
Spell,6343,Thunder Clap,F9
Spell,2565,Shield Block,D4
Spell,190456,Ignore Pain,D5
Spell,203526,Neltharion's Fury,F8
Spell,6552,Pummel,F3
Spell,34428,Victory Rush,D9
Spell,46968,Shockwave,None
Spell,202168,Impending Victory,D9
Spell,871,Shield Wall,F6
Spell,12975,Last Stand,F5
Spell,1160,Demoralizing Shout,F1
Spell,23920,SpellReflect,D0
Spell,1719,Battle Cry,F2
Spell,0,HealthPotion,D7
Item,127834,HealthPotion,D7
Aura,2565,Shield Block
Aura,132168,ShockWavestun
Aura,202573,Vengeance Revenge
Aura,202574,Vengeance Ignore Pain
Aura,190456,Ignore Pain
Aura,203576,Dragon Scales
*/
