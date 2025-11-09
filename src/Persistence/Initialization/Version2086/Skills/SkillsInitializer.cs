// <copyright file="SkillsInitializer.cs" company="MUnique">
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

namespace MUnique.OpenMU.Persistence.Initialization.Version2086.Skills;

// ReSharper disable StringLiteralTypo
using System;
using System.Reflection.PortableExecutable;
using MUnique.OpenMU.AttributeSystem;
using MUnique.OpenMU.DataModel.Configuration;
using MUnique.OpenMU.GameLogic.Attributes;
using MUnique.OpenMU.GameLogic.NPC;
using MUnique.OpenMU.Persistence.Initialization.Skills;
using MUnique.OpenMU.Persistence.Initialization.Version2086.CharacterClasses;

/// <summary>
/// Initialization logic for <see cref="Skill"/>s.
/// </summary>
internal class SkillsInitializer : SkillsInitializerBase
{
    internal const string Formula1WhenComplete = "if(level < 10; 0; 1)";

    private static readonly IDictionary<int, string> Formulas = new Dictionary<int, string>()
    {
        { 0, string.Empty },
        { 1, "1+((((((level )-30)^3)+25000)/499)/6)" },
        { 2, "0.8+(((((((level -30)^3)+25000)/499)/6)))" },
        { 3, "(0.85+(((((((level -30)^3)+25000)/499)/6))))*6" },
        { 4, "(0.9+(((((((level -30)^3)+25000)/499)/6))))*8" },
        { 5, "(0.95+(((((((level -30)^3)+25000)/499)/6))))*10" },
        { 6, "52/(1+(((((((level -30)^3)+25000)/499)/6))))" },
        { 7, "(1+(((((((level -30)^3)+25000)/499)/6))))*1.5" },
        { 8, "(1+(((((((level -30)^3)+25000)/499)/50)*100)/12))*23" },
        { 9, "(1+(((((((level -30)^3)+25000)/499)/50)*100)/12))*85" },
        { 10, "(0.8+(((((((level -30)^3)+25000)/499)/50)*100)/12))*8" },
        { 11, "11/(1+(((((((level -30)^3)+25000)/499)/50)*100)/12))" },
        { 12, "(1+(((((((level -30)^3)+25000)/499)/50)*100)/12))*85*6" },
        { 13, "(1+(((((((level -30)^3)+25000)/499)/50)*100)/12))*85*5" },
        { 14, "(1+(((((((level -30)^3)+25000)/499)/50)*100)/12))*85*8" },
        { 15, "(1+(((((((level -30)^3)+25000)/499)/6))))*15" },
        { 16, "(1+(((((((level -30)^3)+25000)/499)/6))))*50" },
        { 17, "(1+(((((((level -30)^3)+25000)/499)/6))))*10" },
        { 18, "(1+(((((((level -30)^3)+25000)/499)/6))))*6" },
        { 19, "50/(1+(((((((level -30)^3)+25000)/499)/6))))+20" },
        { 20, "41/(1+(((((((level -29)^3)+22333)/444)/10))))+0.8" },
        { 21, "(2+(((((((level -30)^3)+25000)/499)/6))))*1.5" },
        { 22, "(0.9+(((((((level -35)^3)+40440)/600)/7.5))))*5" },
        { 23, "level *1.0" },
        { 24, "(0.95+(((((((level -30)^3)+25000)/499)/6))))*7.8" },
        { 25, "170" },
        { 26, "150" },
        { 27, "130" },
        { 28, "800" },
        { 29, "(1+(((((((level -30)^3)+25000)/499)/50)*100)/12))*85*2.5" },
        { 30, "(1+(((((((level -30)^3)+25000)/499)/50)*100)/12))*85*2" },
        { 31, "(1+(((((((level -30)^3)+25000)/499)/50)*100)/12))*85*3.2" },
        { 32, "(1+(((((((level -30)^3)+25000)/499)/50)*100)/12))*85*5" },
        { 33, "(1+(((((((level -30)^3)+25000)/499)/50)*100)/12))*85*3" },
        { 34, "(1+(((((((level -30)^3)+25000)/499)/50)*100)/12))*45" },
        { 35, "(1+(((((((level -30)^3)+25000)/499)/6))))*28" },
        { 36, "(1+(((((((level -30)^3)+25000)/499)/6))))*8" },
        { 37, "(1+(((((((level -30)^3)+25000)/499)/50)*100)/12))*15" },
        { 38, "0.1+(((((((level -47)^3)+120000)/600)/41)))" },
        { 39, "0.9+(((((((level -30)^3)+25000)/499)/12)))" },
        { 40, "(1+((((((((level )-30)^3)+25000)/499)/6))))*12" },
        { 41, "(1+((((((((level )-30)^3)+25000)/499)/6))))*13" },
        { 42, "(1+((((((((level )-30)^3)+25000)/499)/6))))*14" },
        { 43, "(1+(((((((level -35)^3)+43000)/499)/50)*100)/12))*85" },
        { 44, "(((((20+level )^1.4)*2.402)/2.8)*4)" },
        { 45, "15/level " },
        { 46, "30/level " },
        { 47, "45/level " },
        { 48, "60/level " },
        { 49, "15/level " },
        { 50, "((((((2.02+level )^1.1)/10)*5)/3)*1)" },
        { 51, "((((((3.33+level )^1.3)/10)*5)/3)*1)" },
        { 52, "((((((3.78+level )^1.42)/10)*5)/3)*1)" },
        { 53, "((((((3.33+level )^1.52)/10)*5)/3)*1)" },
        { 54, "level *1" },
        { 55, "(level *15/10)" },
        { 56, "(10/level )*7" },
        { 57, "(10/level )*10" },
        { 58, "(10/level )*11" },
        { 59, "(10/level )*12" },
        { 60, "10/level " },
        { 61, "10/level " },
        { 62, "10/level " },
        { 63, "10/level " },
        { 64, "10/level " },
        { 65, "10/level " },
        { 66, "level *0.75" },
        { 67, "level *1" },
        { 68, "10/level " },
        { 69, "10/level " },
        { 70, "((((((20+level )^2)/15)*7.5)/213.3)*4)" },
        { 71, "((((((20+level )^2)/15)*7.5)/213.3)*4)" },
        { 72, "((((((20+level )^2)/15)*7.5)/213.3)*4)" },
        { 73, "((((((20+level )^2)/15)*7.5)/213.3)*4)" },
        { 74, "87+((level *6.5)^1.6)" },
        { 75, "((((((5.85+level )^2)/10)*5)/5)*3)+15" },
        { 76, "(140+((level *18)^1.265))" },
        { 77, "((((((7+level )^2)/10)*5)/9.8)*5)/10" },
        { 78, "level *1" },
        { 79, "level *1" },
        { 80, "level *1" },
        { 81, "level *1" },
        { 82, "(125+((level *7.5)^1.5))" },
        { 83, "0.91+(level *0.278)^1.3" },
        { 84, "(96+((level *21.9)^1.2))" },
        { 85, "level /5" },
        { 86, "level /10" },
        { 87, "level /10" },
        { 88, "level /10" },
        { 89, "level /10" },
        { 90, "level *0.5" },
        { 91, "0.94+(level *0.3)^1.23" },
        { 92, "0.94+(level *0.3)^1.23" },
        { 93, "0.94+(level *0.3)^1.23" },
        { 94, "0.94+(level *0.3)^1.23" },
        { 95, "((((((4+level )^2)/10)*5)/7)*5*8)" },
        { 96, "((((((5.9+level )^2)/10)*5)/10)*3)" },
        { 97, "((((((5+level )^2)/10)*5)/7)*4*7)" },
        { 98, "((((((4.25+level )^2)/10)*5)/9.8)*5)/15" },
        { 99, "level *1" },
        { 100, "level *1" },
        { 101, "level *1" },
        { 102, "level *1" },
        { 103, "(108+((level *5.1)^1.5))" },
        { 104, "0.83+(level *0.278)^1.25" },
        { 105, "(95+((level *14)^1.2))" },
        { 106, "level /10" },
        { 107, "((((((20+level )^2)/15)*5)/213.3)*2)" },
        { 108, "((((((20+level )^2)/15)*5)/213.3)*2)" },
        { 109, "((((((20+level )^2)/15)*5)/213.3)*2)" },
        { 110, "((((((20+level )^2)/15)*5)/213.3)*2)" },
        { 111, "level *0.5" },
        { 112, "((((((23+level )^2)/10)*5)/7)*4)" },
        { 113, "((((((8+level )^2)/10)*5)/7)*4)" },
        { 114, "((((((9+level )^2)/10)*5)/7)*4)" },
        { 115, "((((((18+level )^2)/10)*5)/7)*4)" },
        { 116, "((((((8+level )^2)/10)*5)/7)*4)" },
        { 117, "((((((6+level )^2)/10)*5)/7)*4)" },
        { 118, "((((((8+level )^2)/10)*5)/7)*4)" },
        { 119, "((((((5+level )^2)/10)*5)/7)*4)" },
        { 120, "((((((38+level )^2)/10)*5)/7)*4)" },
        { 121, "((((((11+level )^2)/10)*5)/7)*4)" },
        { 122, "((((((10+level )^2)/10)*5)/7)*4)" },
        { 123, "((((((32+level )^2)/10)*5)/7)*4)" },
        { 124, "((((((9+level )^2)/10)*5)/7)*4)" },
        { 125, "((((((7+level )^2)/10)*5)/7)*4)" },
        { 126, "((((((7+level )^2)/10)*5)/7)*5)" },
        { 127, "((((((26+level )^2)/10)*5)/7)*4)" },
        { 128, "((((((5+level )^2)/10)*5)/7)*4)" },
        { 129, "((((((4+level )^2)/10)*5)/7)*4)" },
        { 130, "((((((22+level )^2)/10)*5)/7)*4)" },
        { 131, "((((((23+level )^2)/10)*5)/7)*4)" },
        { 132, "((((((22+level )^2)/10)*5)/7)*4)" },
        { 133, "((((((26+level )^2)/10)*5)/7)*4)" },
        { 134, "((((((25+level )^1.7)/10)*9)/7)*4)" },
        { 135, "((((((3+level )^2)/10)*5)/7)*4)" },
        { 136, "((((((23+level )^2)/10)*5)/7)*4)" },
        { 137, "((((((12+level )^2)/10)*5)/7)*4)" },
        { 138, "((((((1+level )^2)/10)*5)/7)*4)" },
        { 139, "(25000+((level -800)*25))+((level +level +level )*0)" },
        { 140, "(25000+((level -800)*25))+((level +level +level )*0)" },
        { 141, "(25000+((level -800)*25))+((level +level +level )*0)" },
        { 142, "(25000+((level -800)*25))+((level +level +level )*0)" },
        { 143, "(((15+level )^2.5))+1253" },
        { 144, "(((15+level )^2.5))+1253" },
        { 145, "(((15+level )^2.5))+1253" },
        { 146, "(((15+level )^2.5))+1253" },
        { 147, "10/level " },
        { 148, "10/level " },
        { 149, "10/level " },
        { 150, "40/level " },
        { 151, "((((((level *level )*0.6/24)+(((level -800)*(level -500))/200))/24)+1500))" },
        { 152, "70*level " },
        { 153, "8+(level *0)" },
        { 154, "15*level " },
        { 155, "7*level " },
        { 156, "13*level " },
        { 157, "13*level " },
        { 158, "8+(level *0)" },
        { 159, "50*level " },
        { 160, "7*level " },
        { 161, "13*level " },
        { 162, "level *3.5" },
        { 163, "level *0" },
        { 164, "4.5/level " },
        { 165, "((((((4+level )^1.6)/7)*5.5)/5.7)*5)" },
        { 166, "((((((5+level )^1.6)/5)*4)/5)*4.3)" },
        { 167, "((((((5+level )^1.6)/5)*4)/4.3)*4.2)" },
        { 168, "10/level " },
        { 169, "10/level " },
        { 170, "10/level " },
        { 171, "20/level " },
        { 172, "(0.9+(((((((level -35)^3)+40440)/600)/7.5))))*5" },
        { 173, "level *1" },
        { 174, "(0.9+(((((((level -35)^3)+40440)/600)/7.5))))*5" },
        { 175, "(1+(((((((level -30)^3)+25000)/499)/6))))*33.3" },
        { 176, "((((((4+level )^1.6)/7)*5.5)/5.7)*5)" },
        { 177, "((((((4+level )^1.6)/7)*5.5)/5.5)*6)" },
        { 178, "300" },
        { 179, "6.073+(level *0.3)^1.47" },
        { 180, "6.073+(level *0.3)^1.47" },
        { 181, "6.073+(level *0.3)^1.47" },
        { 182, "6.073+(level *0.3)^1.47" },
        { 183, "level /10" },
        { 184, "level /10" },
        { 185, "level /10" },
        { 186, "level /10" },
        { 187, "level /10" },
        { 188, "level /10" },
        { 189, "level /10" },
        { 190, "level /10" },
        { 191, "level /10" },
        { 192, "level /10" },
        { 193, "level /10" },
        { 194, "level /10" },
        { 195, "(19.75+((level ^1.3)*5))" },
        { 196, "(14.71+((level ^1.3)*4))" },
        { 197, "(19.75+((level ^1.3)*5))" },
        { 198, "(14.71+((level ^1.3)*4))" },
        { 199, "(19.75+((level ^1.3)*5))" },
        { 200, "(14.71+((level ^1.3)*4))" },
        { 201, "(5.71+((level ^1.3)*4))" },
        { 202, "(5.71+((level ^1.3)*4))" },
        { 203, "(5.71+((level ^1.3)*4))" },
        { 204, "(5.45+((level ^1.3)*3))" },
        { 205, "(5.45+((level ^1.3)*3))" },
        { 206, "(5.45+((level ^1.3)*3))" },
        { 207, "level /10" },
        { 208, "level /10" },
        { 209, "level /10" },
        { 210, "7*level " },
        { 211, "7*level " },
        { 212, "7*level " },
        { 213, "(((((20+level )^1.4)*2.402)/4.5))" },
        { 214, "(1+(((((((level -30)^3)+25000)/490)/50)*100)/15))*20" },
        { 215, "(((15+level )^2.55))+1843" },
        { 216, "(((15+level )^2.55))+1843" },
        { 217, "(((15+level )^2.55))+1843" },
        { 218, "level /10" },
        { 219, "level /10" },
        { 220, "level /10" },
        { 221, "15/level " },
        { 222, "2100+((level -30)*42)" },
        { 223, "450+((level -30)*9)" },
        { 224, "390+((level -30)*8)" },
        { 225, "1500+((level -30)*30)" },
        { 226, "level *0" },
        { 227, "level /10" },
        { 228, "1.91+((level ^1.4)/3)" },
        { 229, "1.01+((level ^1.3)/8.2)" },
        { 230, "3.1+(((((((level -50)^3)+120000)/60)/33)))" },
        { 231, "10+((((level +10)^2)/4)-25)/2.5" },
        { 232, "level /10" },
        { 233, "(((level +10)^2)/6)" },
        { 234, "(((level +10)^2)/3)" },
        { 235, "((((((23+level )^2)/10)*5)/7)*2)" },
        { 236, "((((((9+level )^2)/10)*5)/7)*1)" },
        { 237, "60+(level *3)" },
        { 238, "((1+(((((((level -30)^3)+25000)/499)/50)*100)/12))*85)/2" },
        { 239, "30+((((level +10)^2)/5)-25)/2" },
        { 240, "1.91+((level ^1.4)/3)" },
        { 241, "1+((level ^1.2)/3.7)" },
        { 242, "((((level +10)^2)/4)-25)/2" },
        { 243, "level *1.5" },
        { 244, "level *0.75" },
        { 245, "((2+((((((((level )-30)^3)+25000)/499)/6))))*12)" },
        { 246, "((1+((((((((level )-30)^3)+25000)/499)/7.5))))*12)" },
        { 247, "((1+((((((((level )-30)^3)+25000)/490)/7))))*12)+10" },
        { 248, "((2+((((((((level )-30)^3)+25000)/499)/6))))*12)" },
        { 249, "((1+(((((((level -30)^3)+25000)/499)/6))))*9.2)" },
        { 250, "((1+(((((((level -30)^3)+25000)/499)/6))))*13)" },
        { 251, "level /10" },
        { 252, "level /10" },
        { 253, "(((((((5+level )^1.5)/6)*5)/4)*3.5)-3.5)" },
        { 254, "((((level ^1.3)*4.1)/3.33))+3" },
        { 255, "(((((level ^1.45)*4)/3)+3.5)*0.85)" },
        { 256, "(((((level ^1.3)*4.1)/3.35)+3.1)*0.8)" },
        { 257, "(((((level ^1.45)*4)/3)+3.5)*0.85)" },
        { 258, "level +1" },
        { 259, "level +1" },
        { 260, "level +1" },
        { 261, "level +1" },
        { 262, "((((level ^1.4)*3)/6)+8)" },
        { 263, "(1+((((((level )-30)^3)+25000)/499)/5.3))-0.07475" },
        { 264, "level +1" },
        { 265, "level +1" },
        { 266, "((((((5+level )^1.6)/4)*4)/5)*4.5)" },
        { 267, "((((((5+level )^1.6)/4)*4)/5)*5)" },
        { 268, "((((((5+level )^1.6)/5)*4)/5)*3.5)" },
        { 269, "(((((level ^1.3)*4.1)/3.35)+3.1)*0.8)" },
        { 270, "(((((level ^1.45)*4)/3)+3.5)*0.85)" },
        { 271, "(0.9+(((((((level -35)^3)+40440)/600)/7.5))))*4" },
        { 272, "(0.85+(((((((level -30)^3)+25000)/499)/50)*100)/12))*6" },
        { 273, "((((((2+level )^2)/10)*6)/7)*7.5)" },
        { 274, "((((((8+level )^2)/10)*4)/7)*9.5)" },
        { 275, "(((((level ^1.3)*6.1)/3)+1.1)*2)" },
        { 276, "(((((level ^1.3)*4.1)/3.35)+3.1)*1.5)" },
        { 277, "(((((level ^1.3)*4.1)/3.35)+3.1)*2.5)" },
        { 278, "level /10" },
        { 279, "level /10" },
        { 280, "level /10" },
        { 281, "(((((level ^1.3)*3.5)/3.35)+3.1)*4.5)" },
        { 282, "(((((level ^1.3)*3.5)/3.35)+3.1)*2.8)" },
        { 283, "(1+((((((level )-30)^3)+25000)/499)/8))-0.07475" },
        { 284, "(1+((((((level )-30)^3)+25000)/499)/8))-0.07475" },
        { 285, "((5+(((level -47)^3)+102000)/860)/2)" },
        { 286, "level *1" },
        { 287, "(1+(((((((level -30)^3)+25000)/499)/6))))*33.3" },
        { 288, "((((((8+level )^2)/10)*7)/6)*4)" },
        { 289, "((((((25+level )^1.7)/10)*9)/7)*4)" },
        { 290, "((((((23+level )^2)/10)*5)/7)*2)" },
        { 291, "(0.8+(((((((level -30)^3)+25000)/499)/50)*100)/12))*8" },
        { 292, "(1+(((((((level -30)^3)+25000)/499)/50)*100)/12))*23" },
        { 293, "(1+(((((((level -30)^3)+25000)/499)/50)*100)/12))*23" },
        { 294, "(((((level ^1.3)*4.1)/3.35)+3.1)*0.8)" },
        { 295, "level /10" },
        { 296, "(((((level ^1.3)*4.1)/3.35)+3.1)*0.8)" },
        { 297, "level /10" },
        { 298, "(((((level ^1.3)*4.1)/3.35)+3.1)*0.8)" },
        { 299, "level /10" },
        { 300, "level /10" },
        { 301, "(((((level ^1.3)*4.1)/3.35)+3.1)*0.8)" },
        { 302, "level *5" },
        { 303, "level *1.0" },
        { 304, "level *0.5" },
        { 305, "(((((level ^1.3)*3.5)/3.35)+3.1)*4.5)" },
        { 306, "(1+((((((level )-30)^3)+25000)/499)/8))-0.07475" },
        { 307, "((((((2+level )^2)/10)*6)/7)*7.5)" },
        { 308, "((((((5+level )^1.6)/4)*4)/5)*4.5)" },
        { 309, "((((((5+level )^1.6)/4)*4)/5)*5)" },
        { 310, "((((((5+level )^1.6)/5)*4)/5)*3.5)" },
        { 311, "((1+(((((((level -30)^3)+25000)/499)/50)*100)/12))*85*2)*2" },
        { 312, "level *1.5" },
        { 313, "(0.9+(((((((level -35)^3)+40440)/450)/3))))*3.3+7" },
        { 314, "((((((5+level )^1.74)/4)*4)/5)*7.5)-20" },
        { 315, "(0.9+(((((((level -35)^3)+40440)/450)/3))))*3.3+7" },
        { 316, "((((((5+level )^1.85)/4)*4)/5)*7.6)+10" },
        { 317, "((((((2+level )^2)/10)*6)/7)*7.5)" },
        { 318, "((0.85+(((level -30)^3)+25000)/499/50*100/12)*6)" },
        { 319, "level *50" },
        { 320, "level *75" },
        { 321, "(((level +9)^1.2/4.4-1.92))" },
        { 322, "((((((5+level )^1.85)/4)*4)/5)*7.6)+20" },
        { 323, "level *1.5" },
        { 324, "(0.9+(((((((level -35)^3)+40440)/450)/3))))*3+7" },
        { 325, "level /10" },
        { 326, "(0.9+(((((((level -35)^3)+40440)/450)/3))))*3+7" },
        { 327, "level /10" },
        { 328, "(0.9+(((((((level -35)^3)+40440)/450)/3))))*3.3+7" },
        { 329, "level /10" },
        { 330, "level *5" },
        { 331, "1+((((((level )-30)^3)+25000)/499)/6)" },
        { 332, "level *1.5" },
        { 333, "((((4+level )^2)/7)*7.5)" },
        { 334, "((((6+level )^1.7)/5)*4.5)" },
        { 335, "((((((5+level )^1.74)/4)*4)/5)*8)" },
    };

    private static readonly IDictionary<SkillNumber, MagicEffectNumber> EffectsOfSkills = new Dictionary<SkillNumber, MagicEffectNumber>
    {
        { SkillNumber.SwellLife, MagicEffectNumber.GreaterFortitude },
        { SkillNumber.LordDignity, MagicEffectNumber.CriticalDamageIncrease },
        { SkillNumber.SoulBarrier, MagicEffectNumber.SoulBarrier },
        { SkillNumber.Defense, MagicEffectNumber.ShieldSkill },
        { SkillNumber.GreaterDefense, MagicEffectNumber.GreaterDefense },
        { SkillNumber.GreaterDamage, MagicEffectNumber.GreaterDamage },
        { SkillNumber.Heal, MagicEffectNumber.Heal },
        { SkillNumber.Recovery, MagicEffectNumber.ShieldRecover },
        { SkillNumber.InfinityArrow, MagicEffectNumber.InfiniteArrow },
        { SkillNumber.FireSlash, MagicEffectNumber.DefenseReduction },
        { SkillNumber.IgnoreDefense, MagicEffectNumber.IgnoreDefense },
        { SkillNumber.IncreaseHealth, MagicEffectNumber.IncreaseHealth },
        { SkillNumber.IncreaseBlock, MagicEffectNumber.IncreaseBlock },
        { SkillNumber.ExpansionofWizardry, MagicEffectNumber.WizEnhance },
        { SkillNumber.Berserker, MagicEffectNumber.Berserker },
    };

    private readonly IDictionary<byte, MasterSkillRoot> _masterSkillRoots;

    /// <summary>
    /// Initializes a new instance of the <see cref="SkillsInitializer"/> class.
    /// </summary>
    /// <param name="context">The persistence context.</param>
    /// <param name="gameConfiguration">The game configuration.</param>
    public SkillsInitializer(IContext context, GameConfiguration gameConfiguration)
        : base(context, gameConfiguration)
    {
        this._masterSkillRoots = new SortedDictionary<byte, MasterSkillRoot>();
    }

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    /// <remarks>
    /// Regex: (?m)^\s*(\d+)\s+\"(.+?)\"\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(-*\d+)\s(-*\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s*$
    /// Replace by: this.CreateSkill($1, "$2", $3, $4, $5, $6, $7, $9, $10, $11, $12, $13, $15, $19, $20, $21, $22, $23, $24, $25, $26, $27, $28);.
    /// </remarks>
    public override void Initialize()
    {
        // DL
        this.CreateSkill(60, "Force", DamageType.Physical, damage: 10, distance: 4, abilityConsumption: 0, manaConsumption: 10, energyRequirement: 15, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 40), darkLordClass: 1);
        this.CreateSkill(61, "Fire Burst", DamageType.Physical, damage: 150, distance: 6, skillTarget: SkillTarget.ExplicitWithImplicitInRange, implicitTargetRange: 1, abilityConsumption: 0, manaConsumption: 8, energyRequirement: 79, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 80), darkLordClass: 1);
        this.CreateSkill(62, "Earth-Shake", DamageType.Physical, damage: 150, distance: 10, abilityConsumption: 50, manaConsumption: 0, elementalModifier: (ElementalType)6, darkLordClass: 1);
        this.CreateSkill(63, "Summon", damage: 0, distance: 0, abilityConsumption: 30, manaConsumption: 70, energyRequirement: 153, leadershipRequirement: 400, darkLordClass: 1);
        this.CreateSkill(64, "Dignity", damage: 0, distance: 0, abilityConsumption: 50, manaConsumption: 50, energyRequirement: 102, leadershipRequirement: 300, darkLordClass: 1);
        this.CreateSkill(65, "Electric Spike", DamageType.Physical, damage: 250, distance: 10, skillType: SkillType.AreaSkillAutomaticHits, abilityConsumption: 12, manaConsumption: 10, energyRequirement: 126, leadershipRequirement: 340, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), darkLordClass: 1);
        this.AddAreaSkillSettings(SkillNumber.ElectricSpark, true, 1.5f, 1.5f, 12f, useDeferredHits: true, delayPerOneDistance: TimeSpan.FromMilliseconds(10));
        this.CreateSkill(66, "Force Wave", DamageType.Physical, damage: 70, distance: 4, abilityConsumption: 0, manaConsumption: 10, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 40), darkLordClass: 1);
        this.CreateSkill(78, "Fire Scream", DamageType.Physical, damage: 180, distance: 6, skillType: SkillType.AreaSkillAutomaticHits, abilityConsumption: 3, manaConsumption: 10, energyRequirement: 150, leadershipRequirement: 70, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), darkLordClass: 1);
        this.AddAreaSkillSettings(SkillNumber.FireScream, true, 2f, 3f, 6f);
        this.CreateSkill(238, "Chaotic Diseier", DamageType.Physical, damage: 220, distance: 6, skillType: SkillType.AreaSkillAutomaticHits, abilityConsumption: 4, manaConsumption: 12, energyRequirement: 84, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 80), darkLordClass: 1);
        this.AddAreaSkillSettings(SkillNumber.ChaoticDiseier, true, 1.5f, 1.5f, 6f);
        this.CreateSkill(2095, "Spirit Blast", DamageType.Physical, damage: 350, distance: 6, skillType: SkillType.AreaSkillAutomaticHits, abilityConsumption: 16, manaConsumption: 35, leadershipRequirement: 620, strengthRequirement: 800, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalLeadership, 50), scalingStat2: (Stats.TotalStrength, 100), darkLordClass: 3);

        // Generic monster skills:
        this.CreateSkill((short)SkillNumber.MonsterSkill, "Generic Monster Skill", distance: 5, skillType: SkillType.Other);

        this.CreateSkill(1, "Poison", DamageType.Wizardry, damage: 12, distance: 6, abilityConsumption: 0, manaConsumption: 42, energyRequirement: 140, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 50), wizardClass: 1, magicGladiatorClass: 1, runeWizardClass: 1, lemuriaClass: 1, alchemistClass: 1);
        this.CreateSkill(2, "Meteorite", DamageType.Wizardry, damage: 21, distance: 6, abilityConsumption: 0, manaConsumption: 12, energyRequirement: 104, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), wizardClass: 1, magicGladiatorClass: 1, summonerClass: 1, runeWizardClass: 1, lemuriaClass: 1, alchemistClass: 1);
        this.CreateSkill(3, "Lightning", DamageType.Wizardry, damage: 17, distance: 6, abilityConsumption: 0, manaConsumption: 15, energyRequirement: 72, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 40), wizardClass: 1, magicGladiatorClass: 1, whiteWizardClass: 1, lemuriaClass: 1, alchemistClass: 1);
        this.CreateSkill(4, "Fire Ball", DamageType.Wizardry, damage: 8, distance: 6, abilityConsumption: 0, manaConsumption: 3, energyRequirement: 40, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 30), wizardClass: 1, magicGladiatorClass: 1, summonerClass: 1, runeWizardClass: 1, lemuriaClass: 1, alchemistClass: 1);
        this.CreateSkill(5, "Flame", DamageType.Wizardry, damage: 25, distance: 6, abilityConsumption: 0, manaConsumption: 50, energyRequirement: 160, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 100), wizardClass: 1, magicGladiatorClass: 1, runeWizardClass: 1, lemuriaClass: 1, alchemistClass: 1);
        this.CreateSkill(6, "Teleport", damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 30, energyRequirement: 88, wizardClass: 1, whiteWizardClass: 1);
        this.CreateSkill(7, "Ice", DamageType.Wizardry, damage: 10, distance: 6, abilityConsumption: 0, manaConsumption: 38, energyRequirement: 120, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), wizardClass: 1, magicGladiatorClass: 1, summonerClass: 1, whiteWizardClass: 1, lemuriaClass: 1, alchemistClass: 1);
        this.CreateSkill(8, "Twister", DamageType.Wizardry, damage: 35, distance: 6, abilityConsumption: 0, manaConsumption: 60, energyRequirement: 180, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 90), wizardClass: 1, magicGladiatorClass: 1, runeWizardClass: 1, whiteWizardClass: 1, lemuriaClass: 1, alchemistClass: 1);
        this.CreateSkill(9, "Evil Spirit", DamageType.Wizardry, damage: 45, distance: 6, abilityConsumption: 0, manaConsumption: 90, energyRequirement: 220, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), wizardClass: 1, magicGladiatorClass: 1, runeWizardClass: 1);
        this.CreateSkill(10, "Hellfire", DamageType.Wizardry, damage: 120, distance: 3, abilityConsumption: 0, manaConsumption: 160, energyRequirement: 260, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), wizardClass: 1, magicGladiatorClass: 1, runeWizardClass: 1, lemuriaClass: 1, alchemistClass: 1);
        this.CreateSkill(11, "Power Wave", DamageType.Wizardry, damage: 14, distance: 6, abilityConsumption: 0, manaConsumption: 5, energyRequirement: 56, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 30), wizardClass: 1, magicGladiatorClass: 1, summonerClass: 1, whiteWizardClass: 1, lemuriaClass: 1, alchemistClass: 1);
        this.CreateSkill(12, "Aqua Beam", DamageType.Wizardry, damage: 80, distance: 6, abilityConsumption: 0, manaConsumption: 140, energyRequirement: 345, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), wizardClass: 1, magicGladiatorClass: 1, whiteWizardClass: 1, alchemistClass: 1);
        this.CreateSkill(13, "Cometfall", DamageType.Wizardry, damage: 70, distance: 6, abilityConsumption: 0, manaConsumption: 150, energyRequirement: 500, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), wizardClass: 1, magicGladiatorClass: 1, runeWizardClass: 1, whiteWizardClass: 1, alchemistClass: 1);
        this.CreateSkill(14, "Inferno", DamageType.Wizardry, damage: 100, distance: 4, abilityConsumption: 0, manaConsumption: 200, energyRequirement: 724, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 90), wizardClass: 1, magicGladiatorClass: 1, runeWizardClass: 1, lemuriaClass: 1, alchemistClass: 1);
        this.CreateSkill(15, "Teleport Ally", damage: 0, distance: 6, abilityConsumption: 25, manaConsumption: 90, energyRequirement: 644, wizardClass: 2, whiteWizardClass: 2, lemuriaClass: 2, alchemistClass: 2);
        this.CreateSkill(16, "Soul Barrier", damage: 0, distance: 6, abilityConsumption: 22, manaConsumption: 70, energyRequirement: 408, wizardClass: 1);
        this.CreateSkill(17, "Energy Ball", DamageType.Wizardry, damage: 3, distance: 6, abilityConsumption: 0, manaConsumption: 1, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 30), wizardClass: 1);
        this.CreateSkill(18, "Defense", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 30, knightClass: 1, magicGladiatorClass: 1, darkLordClass: 1, growLancerClass: 1, crusaderClass: 1);
        this.CreateSkill(19, "Falling Slash", DamageType.Physical, damage: 0, distance: 3, abilityConsumption: 0, manaConsumption: 9, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 30), knightClass: 1, magicGladiatorClass: 1, darkLordClass: 1, ragefighterClass: 1, crusaderClass: 1);
        this.CreateSkill(20, "Lunge", DamageType.Physical, damage: 0, distance: 2, abilityConsumption: 0, manaConsumption: 9, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 30), knightClass: 1, magicGladiatorClass: 1, darkLordClass: 1, growLancerClass: 1, slayerClass: 1);
        this.CreateSkill(21, "Uppercut", DamageType.Physical, damage: 0, distance: 2, abilityConsumption: 0, manaConsumption: 8, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 30), knightClass: 1, magicGladiatorClass: 1, darkLordClass: 1, slayerClass: 1);
        this.CreateSkill(22, "Cyclone", DamageType.Physical, damage: 0, distance: 2, abilityConsumption: 0, manaConsumption: 9, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 30), knightClass: 1, magicGladiatorClass: 1, darkLordClass: 1, slayerClass: 1, illusionKnightClass: 1);
        this.CreateSkill(23, "Slash", DamageType.Physical, damage: 0, distance: 2, abilityConsumption: 0, manaConsumption: 10, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 30), knightClass: 1, magicGladiatorClass: 1);
        this.CreateSkill(24, "Triple Shot", DamageType.Physical, damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 5, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), elfClass: 1);
        this.CreateSkill(26, "Heal", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 20, energyRequirement: 52, elfClass: 1);
        this.CreateSkill(27, "Greater Defense", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 30, energyRequirement: 72, elfClass: 1);
        this.CreateSkill(28, "Greater Damage", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 40, energyRequirement: 92, elfClass: 1);
        this.CreateSkill(30, "Summon Goblin", damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 40, energyRequirement: 30, elfClass: 1);
        this.CreateSkill(31, "Summon Stone Golem", damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 70, energyRequirement: 60, elfClass: 1);
        this.CreateSkill(32, "Summon Assassin", damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 110, energyRequirement: 90, elfClass: 1);
        this.CreateSkill(33, "Summon Elite Yeti", damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 160, energyRequirement: 130, elfClass: 1);
        this.CreateSkill(34, "Summon Dark Knight", damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 200, energyRequirement: 170, elfClass: 1);
        this.CreateSkill(35, "Summon Bali", damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 250, energyRequirement: 210, elfClass: 1);
        this.CreateSkill(36, "Summon Soldier", damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 350, energyRequirement: 300, elfClass: 1);
        this.CreateSkill(38, "Decay", DamageType.Wizardry, damage: 95, distance: 6, abilityConsumption: 7, manaConsumption: 110, energyRequirement: 953, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 100), wizardClass: 2, lemuriaClass: 2, alchemistClass: 2);
        this.CreateSkill(39, "Ice Storm", DamageType.Wizardry, damage: 80, distance: 6, abilityConsumption: 5, manaConsumption: 100, energyRequirement: 849, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 90), wizardClass: 2, runeWizardClass: 2, lemuriaClass: 2, alchemistClass: 2);
        this.CreateSkill(40, "Nova", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 45, manaConsumption: 180, energyRequirement: 1052, elementalModifier: (ElementalType)6, wizardClass: 2, whiteWizardClass: 2, alchemistClass: 2);
        this.CreateSkill(41, "Twisting Slash", DamageType.Physical, damage: 0, distance: 2, abilityConsumption: 10, manaConsumption: 10, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 40), knightClass: 1, magicGladiatorClass: 1, slayerClass: 1, crusaderClass: 1);
        this.CreateSkill(42, "Anger Strike", DamageType.Physical, damage: 60, distance: 3, abilityConsumption: 20, manaConsumption: 25, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), knightClass: 2, slayerClass: 2, crusaderClass: 2);
        this.CreateSkill(43, "Death Stab", DamageType.Physical, damage: 70, distance: 3, abilityConsumption: 3, manaConsumption: 15, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), knightClass: 2);
        this.CreateSkill(44, "Crescent Moon Slash", damage: 90, distance: 4, abilityConsumption: 15, manaConsumption: 22, knightClass: 1, slayerClass: 1, illusionKnightClass: 1);
        this.CreateSkill(45, "Lance", damage: 90, distance: 6, abilityConsumption: 10, manaConsumption: 150, wizardClass: 1, summonerClass: 1, runeWizardClass: 1, whiteWizardClass: 1, lemuriaClass: 1, alchemistClass: 1);
        this.CreateSkill(46, "Starfall", damage: 120, distance: 8, abilityConsumption: 15, manaConsumption: 20, elfClass: 1);
        this.CreateSkill(48, "Swell Life", damage: 0, distance: 0, abilityConsumption: 24, manaConsumption: 22, knightClass: 1);
        this.CreateSkill(49, "Fire Breath", DamageType.Physical, damage: 30, distance: 3, abilityConsumption: 0, manaConsumption: 9, elementalModifier: (ElementalType)6, knightClass: 1);
        this.CreateSkill(50, "Flame of Evil (Monster)", damage: 120, distance: 0, abilityConsumption: 0, manaConsumption: 160, energyRequirement: 260);
        this.CreateSkill(51, "Ice Arrow", DamageType.Physical, damage: 105, distance: 8, abilityConsumption: 12, manaConsumption: 10, dexterityRequirement: 646, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 150), elfClass: 2);
        this.CreateSkill(52, "Penetration", DamageType.Physical, damage: 70, distance: 6, abilityConsumption: 9, manaConsumption: 7, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 200), elfClass: 1);
        this.CreateSkill(55, "Fire Slash", DamageType.Physical, damage: 80, distance: 3, abilityConsumption: 12, manaConsumption: 17, strengthRequirement: 596, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), magicGladiatorClass: 1);
        this.CreateSkill(56, "Power Slash", DamageType.Physical, damage: 0, distance: 5, abilityConsumption: 0, manaConsumption: 15, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), magicGladiatorClass: 1);
        this.CreateSkill(57, "Spiral Slash", damage: 75, distance: 5, abilityConsumption: 15, manaConsumption: 20, magicGladiatorClass: 1);
        this.CreateSkill(67, "Stun", damage: 0, distance: 2, abilityConsumption: 50, manaConsumption: 70, wizardClass: 1, knightClass: 1, elfClass: 1, magicGladiatorClass: 1, darkLordClass: 1, summonerClass: 1, runeWizardClass: 1, slayerClass: 1, whiteWizardClass: 1, lemuriaClass: 1, illusionKnightClass: 1, alchemistClass: 1, crusaderClass: 1);
        this.CreateSkill(68, "Cancel Stun", damage: 0, distance: 0, abilityConsumption: 30, manaConsumption: 25, wizardClass: 1, knightClass: 1, elfClass: 1, magicGladiatorClass: 1, darkLordClass: 1, summonerClass: 1, runeWizardClass: 1, slayerClass: 1, whiteWizardClass: 1, lemuriaClass: 1, illusionKnightClass: 1, alchemistClass: 1, crusaderClass: 1);
        this.CreateSkill(69, "Swell Mana", damage: 0, distance: 0, abilityConsumption: 30, manaConsumption: 35, wizardClass: 1, knightClass: 1, elfClass: 1, magicGladiatorClass: 1, darkLordClass: 1, summonerClass: 1, runeWizardClass: 1, slayerClass: 1, whiteWizardClass: 1, lemuriaClass: 1, illusionKnightClass: 1, alchemistClass: 1, crusaderClass: 1);
        this.CreateSkill(70, "Invisibility", damage: 0, distance: 0, abilityConsumption: 60, manaConsumption: 80, wizardClass: 1, knightClass: 1, elfClass: 1, magicGladiatorClass: 1, darkLordClass: 1, summonerClass: 1, runeWizardClass: 1, slayerClass: 1, whiteWizardClass: 1, lemuriaClass: 1, illusionKnightClass: 1, alchemistClass: 1, crusaderClass: 1);
        this.CreateSkill(71, "Cancel Invisibility", damage: 0, distance: 0, abilityConsumption: 30, manaConsumption: 40, wizardClass: 1, knightClass: 1, elfClass: 1, magicGladiatorClass: 1, darkLordClass: 1, summonerClass: 1, runeWizardClass: 1, slayerClass: 1, whiteWizardClass: 1, lemuriaClass: 1, illusionKnightClass: 1, alchemistClass: 1, crusaderClass: 1);
        this.CreateSkill(72, "Abolish Magic", damage: 0, distance: 0, abilityConsumption: 70, manaConsumption: 90, darkLordClass: 1);
        this.CreateSkill(73, "Mana Rays", damage: 85, distance: 6, abilityConsumption: 7, manaConsumption: 130, magicGladiatorClass: 1);
        this.CreateSkill(74, "Fire Blast", damage: 150, distance: 6, abilityConsumption: 10, manaConsumption: 30, darkLordClass: 1, crusaderClass: 1);
        this.CreateSkill(76, "Plasma Storm", damage: 60, distance: 6, abilityConsumption: 20, manaConsumption: 50, wizardClass: 2, knightClass: 2, elfClass: 2, magicGladiatorClass: 1, darkLordClass: 1, summonerClass: 2, ragefighterClass: 1, growLancerClass: 1, runeWizardClass: 2, slayerClass: 2, gunCrusherClass: 2, whiteWizardClass: 2, lemuriaClass: 2, illusionKnightClass: 2, alchemistClass: 2, crusaderClass: 2);
        this.CreateSkill(77, "Infinity Arrow", damage: 0, distance: 0, abilityConsumption: 10, manaConsumption: 50, elfClass: 2);
        this.CreateSkill(79, "Explosion", damage: 0, distance: 2, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 1);
        this.CreateSkill(200, "Summon Monster", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 40, energyRequirement: 20);
        this.CreateSkill(201, "Magic Attack Immunity", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 40, energyRequirement: 20);
        this.CreateSkill(202, "Physical Attack Immunity", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 40, energyRequirement: 20);
        this.CreateSkill(203, "Potion of Bless", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 40, energyRequirement: 20);
        this.CreateSkill(204, "Potion of Soul", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 40, energyRequirement: 20);
        this.CreateSkill(210, "Spell of Protection", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 30, wizardClass: 1, knightClass: 1, elfClass: 1, magicGladiatorClass: 1, darkLordClass: 1, summonerClass: 1, ragefighterClass: 1, growLancerClass: 1, runeWizardClass: 1, slayerClass: 1, whiteWizardClass: 1, lemuriaClass: 1, illusionKnightClass: 1, alchemistClass: 1, crusaderClass: 1);
        this.CreateSkill(211, "Spell of Restriction", damage: 0, distance: 3, abilityConsumption: 0, manaConsumption: 30, wizardClass: 1, knightClass: 1, elfClass: 1, magicGladiatorClass: 1, darkLordClass: 1, summonerClass: 1, ragefighterClass: 1, growLancerClass: 1, runeWizardClass: 1, slayerClass: 1, whiteWizardClass: 1, lemuriaClass: 1, illusionKnightClass: 1, alchemistClass: 1, crusaderClass: 1);
        this.CreateSkill(212, "Spell of Pursuit", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 30, wizardClass: 1, knightClass: 1, elfClass: 1, magicGladiatorClass: 1, darkLordClass: 1, summonerClass: 1, ragefighterClass: 1, growLancerClass: 1, runeWizardClass: 1, slayerClass: 1, whiteWizardClass: 1, lemuriaClass: 1, illusionKnightClass: 1, alchemistClass: 1, crusaderClass: 1);
        this.CreateSkill(213, "Shied-Burn", damage: 0, distance: 3, abilityConsumption: 0, manaConsumption: 30, wizardClass: 1, knightClass: 1, elfClass: 1, magicGladiatorClass: 1, darkLordClass: 1, summonerClass: 1, ragefighterClass: 1, growLancerClass: 1, runeWizardClass: 1, slayerClass: 1, whiteWizardClass: 1, lemuriaClass: 1, illusionKnightClass: 1, alchemistClass: 1, crusaderClass: 1);
        this.CreateSkill(214, "Drain Life", DamageType.Wizardry, damage: 35, distance: 6, abilityConsumption: 0, manaConsumption: 50, energyRequirement: 150, elementalModifier: (ElementalType)6, summonerClass: 1);
        this.CreateSkill(215, "Chain Lightning", DamageType.Wizardry, damage: 70, distance: 6, abilityConsumption: 0, manaConsumption: 85, energyRequirement: 245, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), summonerClass: 1);
        this.CreateSkill(217, "Damage Reflection", damage: 0, distance: 5, abilityConsumption: 10, manaConsumption: 40, energyRequirement: 375, summonerClass: 1);
        this.CreateSkill(218, "Berserker", damage: 0, distance: 5, abilityConsumption: 50, manaConsumption: 100, energyRequirement: 300, summonerClass: 1);
        this.CreateSkill(219, "Sleep", damage: 0, distance: 6, abilityConsumption: 3, manaConsumption: 20, energyRequirement: 180, summonerClass: 1);
        this.CreateSkill(221, "Weakness", damage: 0, distance: 8, abilityConsumption: 15, manaConsumption: 50, energyRequirement: 663, summonerClass: 1);
        this.CreateSkill(222, "Innovation", damage: 0, distance: 8, abilityConsumption: 15, manaConsumption: 50, energyRequirement: 663, summonerClass: 1);
        this.CreateSkill(223, "Explosion", DamageType.Curse, damage: 40, distance: 4, abilityConsumption: 0, manaConsumption: 35, energyRequirement: 300, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), summonerClass: 1);
        this.CreateSkill(224, "Requiem", DamageType.Curse, damage: 65, distance: 4, abilityConsumption: 4, manaConsumption: 60, energyRequirement: 416, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), summonerClass: 1);
        this.CreateSkill(225, "Pollution", DamageType.Curse, damage: 80, distance: 4, abilityConsumption: 8, manaConsumption: 70, energyRequirement: 542, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), summonerClass: 1);
        this.CreateSkill(230, "Lightning Shock", DamageType.Wizardry, damage: 95, distance: 6, abilityConsumption: 7, manaConsumption: 115, energyRequirement: 823, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), summonerClass: 1);
        this.CreateSkill(232, "Strike of Destruction", DamageType.Physical, damage: 110, distance: 5, abilityConsumption: 24, manaConsumption: 30, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), knightClass: 2, crusaderClass: 2);
        this.CreateSkill(233, "Expansion of Wizardry", damage: 0, distance: 6, abilityConsumption: 50, manaConsumption: 200, energyRequirement: 1058, wizardClass: 2, runeWizardClass: 2, whiteWizardClass: 2, lemuriaClass: 2, alchemistClass: 2);
        this.CreateSkill(234, "Recovery", damage: 0, distance: 6, abilityConsumption: 10, manaConsumption: 40, energyRequirement: 168, elfClass: 2);
        this.CreateSkill(235, "Multi-Shot", DamageType.Physical, damage: 40, distance: 6, abilityConsumption: 7, manaConsumption: 10, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 200), elfClass: 2);
        this.CreateSkill(236, "Flame Strike", DamageType.Physical, damage: 140, distance: 3, abilityConsumption: 25, manaConsumption: 20, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 200), magicGladiatorClass: 1);
        this.CreateSkill(237, "Gigantic Storm", DamageType.Wizardry, damage: 110, distance: 6, abilityConsumption: 10, manaConsumption: 120, energyRequirement: 1058, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 250), magicGladiatorClass: 1);
        this.CreateSkill(239, "Doppeganger Self Explosion", damage: 140, distance: 3, abilityConsumption: 25, manaConsumption: 20, elementalModifier: (ElementalType)6, magicGladiatorClass: 1);
        this.CreateSkill(240, "Magical Shot", DamageType.Wizardry, damage: 30, distance: 6, abilityConsumption: 0, manaConsumption: 1, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 30), runeWizardClass: 1, whiteWizardClass: 1, lemuriaClass: 1, alchemistClass: 1);
        this.CreateSkill(241, "Shining Bird", DamageType.Wizardry, damage: 130, distance: 6, abilityConsumption: 0, manaConsumption: 5, energyRequirement: 70, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), whiteWizardClass: 1);
        this.CreateSkill(242, "Dragon Violent", DamageType.Wizardry, damage: 140, distance: 6, abilityConsumption: 0, manaConsumption: 30, energyRequirement: 680, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), whiteWizardClass: 1);
        this.CreateSkill(243, "Spear Storm", DamageType.Wizardry, damage: 160, distance: 6, abilityConsumption: 5, manaConsumption: 105, energyRequirement: 1160, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), whiteWizardClass: 3);
        this.CreateSkill(244, "Reflection Barrier", damage: 0, distance: 6, abilityConsumption: 22, manaConsumption: 70, energyRequirement: 408, whiteWizardClass: 1);
        this.CreateSkill(245, "Marvel Burst", DamageType.Wizardry, damage: 125, distance: 6, abilityConsumption: 0, manaConsumption: 5, energyRequirement: 104, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), lemuriaClass: 1);
        this.CreateSkill(246, "Unleash Marvel", DamageType.Wizardry, damage: 135, distance: 6, abilityConsumption: 0, manaConsumption: 50, energyRequirement: 700, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), lemuriaClass: 1);
        this.CreateSkill(247, "Ultimate Force", DamageType.Wizardry, damage: 10, distance: 7, abilityConsumption: 4, manaConsumption: 92, energyRequirement: 1073, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), lemuriaClass: 3);
        this.CreateSkill(248, "Reflection Barrier (Reflection Barrier)", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0);
        this.CreateSkill(249, "Illusion Avatar Attack", damage: 300, distance: 7, abilityConsumption: 0, manaConsumption: 0, scalingStat1: (Stats.TotalAgility, 80));
        this.CreateSkill(260, "Killing Blow", DamageType.Physical, damage: 0, distance: 2, abilityConsumption: 0, manaConsumption: 9, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalVitality, 200), ragefighterClass: 1);
        this.CreateSkill(261, "Beast Uppercut", DamageType.Physical, damage: 0, distance: 2, abilityConsumption: 0, manaConsumption: 9, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalVitality, 200), ragefighterClass: 1);
        this.CreateSkill(262, "Chain Drive", DamageType.Physical, damage: 0, distance: 4, abilityConsumption: 20, manaConsumption: 15, elementalModifier: (ElementalType)6, ragefighterClass: 1);
        this.CreateSkill(263, "Dark Side", DamageType.Physical, damage: 0, distance: 5, abilityConsumption: 0, manaConsumption: 70, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), ragefighterClass: 1);
        this.CreateSkill(264, "Dragon Roar", DamageType.Physical, damage: 0, distance: 3, abilityConsumption: 30, manaConsumption: 50, elementalModifier: (ElementalType)6, ragefighterClass: 1);
        this.CreateSkill(265, "Dragon Slasher", DamageType.Physical, damage: 0, distance: 4, abilityConsumption: 100, manaConsumption: 100, elementalModifier: (ElementalType)6, ragefighterClass: 1);
        this.CreateSkill(266, "Ignore Defense", damage: 0, distance: 3, abilityConsumption: 10, manaConsumption: 50, energyRequirement: 404, ragefighterClass: 1);
        this.CreateSkill(267, "Increase Health", damage: 0, distance: 7, abilityConsumption: 10, manaConsumption: 50, energyRequirement: 132, ragefighterClass: 1);
        this.CreateSkill(268, "Increase Block", damage: 0, distance: 7, abilityConsumption: 10, manaConsumption: 50, energyRequirement: 80, ragefighterClass: 1);
        this.CreateSkill(269, "Charge", damage: 90, distance: 4, abilityConsumption: 15, manaConsumption: 20, ragefighterClass: 1);
        this.CreateSkill(270, "Phoenix Shot", DamageType.Physical, damage: 0, distance: 4, abilityConsumption: 0, manaConsumption: 30, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalVitality, 200), ragefighterClass: 1);
        this.CreateSkill(271, "Spin Step", DamageType.Physical, damage: 100, distance: 2, abilityConsumption: 0, manaConsumption: 12, dexterityRequirement: 150, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, -1), growLancerClass: 1);
        this.CreateSkill(272, "Circle Shield", damage: 0, distance: 0, abilityConsumption: 50, manaConsumption: 100, scalingStat1: (Stats.TotalAgility, -1), growLancerClass: 1);
        this.CreateSkill(273, "Obsidian", damage: 0, distance: 0, abilityConsumption: 50, manaConsumption: 50, energyRequirement: 200, scalingStat1: (Stats.TotalStrength, -1), growLancerClass: 1);
        this.CreateSkill(274, "Magic Pin", DamageType.Physical, damage: 80, distance: 2, abilityConsumption: 3, manaConsumption: 5, dexterityRequirement: 200, strengthRequirement: 200, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), growLancerClass: 1);
        this.CreateSkill(275, "Clash", damage: 50, distance: 6, abilityConsumption: 50, manaConsumption: 50, scalingStat1: (Stats.TotalStrength, -1), growLancerClass: 1);
        this.CreateSkill(276, "Harsh Strike", DamageType.Physical, damage: 100, distance: 3, abilityConsumption: 0, manaConsumption: 12, strengthRequirement: 150, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 30), growLancerClass: 1);
        this.CreateSkill(277, "Shining Peak", DamageType.Physical, damage: 50, distance: 4, abilityConsumption: 5, manaConsumption: 8, strengthRequirement: 600, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 1), scalingStat2: (Stats.TotalStrength, 5), growLancerClass: 1);
        this.CreateSkill(278, "Wrath", damage: 0, distance: 0, abilityConsumption: 30, manaConsumption: 40, dexterityRequirement: 200, strengthRequirement: 200, scalingStat1: (Stats.TotalAgility, -1), growLancerClass: 1);
        this.CreateSkill(279, "Breche", DamageType.Physical, damage: 230, distance: 5, abilityConsumption: 6, manaConsumption: 15, dexterityRequirement: 300, strengthRequirement: 300, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), growLancerClass: 1);
        this.CreateSkill(280, "Explosion", damage: 50, distance: 2, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 1);
        this.CreateSkill(281, "Magic Pin Explosion", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 1);
        this.CreateSkill(282, "Spirit Hook", DamageType.Physical, damage: 255, distance: 3, abilityConsumption: 21, manaConsumption: 27, vitalityRequirement: 1480, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalVitality, 10), ragefighterClass: 3);
        this.CreateSkill(283, "Magic Arrow", DamageType.Wizardry, damage: 10, distance: 8, abilityConsumption: 0, manaConsumption: 5, energyRequirement: 60, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 500), runeWizardClass: 1);
        this.CreateSkill(284, "Plasma Ball", DamageType.Wizardry, damage: 40, distance: 8, abilityConsumption: 35, manaConsumption: 120, energyRequirement: 300, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), runeWizardClass: 1);
        this.CreateSkill(285, "Lightning Storm", DamageType.Wizardry, damage: 300, distance: 8, abilityConsumption: 7, manaConsumption: 80, energyRequirement: 1080, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 100), runeWizardClass: 3);
        this.CreateSkill(286, "Burst", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 50, runeWizardClass: 2);
        this.CreateSkill(287, "Haste", damage: 0, distance: 0, abilityConsumption: 50, manaConsumption: 0, runeWizardClass: 2);
        this.CreateSkill(288, "Death Scythe", DamageType.Curse, damage: 75, distance: 6, abilityConsumption: 20, manaConsumption: 120, energyRequirement: 930, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), summonerClass: 3);
        this.CreateSkill(289, "Darkness", damage: 0, distance: 5, abilityConsumption: 50, manaConsumption: 100, energyRequirement: 300, summonerClass: 1);
        this.CreateSkill(291, "Elite Monster Skill", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0);
        this.CreateSkill(292, "Sword Inertia", DamageType.Physical, damage: 10, distance: 6, abilityConsumption: 0, manaConsumption: 5, dexterityRequirement: 100, strengthRequirement: 50, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 150), slayerClass: 1);
        this.CreateSkill(293, "Bat Flock", DamageType.Physical, damage: 90, distance: 6, abilityConsumption: 5, manaConsumption: 20, dexterityRequirement: 380, strengthRequirement: 100, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 150), slayerClass: 2);
        this.CreateSkill(294, "Pierce Attack", DamageType.Physical, damage: 170, distance: 6, abilityConsumption: 10, manaConsumption: 30, dexterityRequirement: 1100, strengthRequirement: 300, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 150), slayerClass: 3);
        this.CreateSkill(295, "Detection", damage: 0, distance: 0, abilityConsumption: 100, manaConsumption: 100, dexterityRequirement: 800, slayerClass: 3);
        this.CreateSkill(297, "Demolish", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 50, dexterityRequirement: 1450, slayerClass: 3);
        this.CreateSkill(298, "Blessing of Experience", damage: 0, distance: 8, abilityConsumption: 0, manaConsumption: 5, wizardClass: 1, knightClass: 1, elfClass: 1, magicGladiatorClass: 1, darkLordClass: 1, summonerClass: 1, ragefighterClass: 1, growLancerClass: 1, runeWizardClass: 1, slayerClass: 1, gunCrusherClass: 1, whiteWizardClass: 1, lemuriaClass: 1, illusionKnightClass: 1, alchemistClass: 1, crusaderClass: 1);
        
        this.CreateSkill(314, "Cast Invincibility", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, illusionKnightClass: 3, crusaderClass: 3);
        this.CreateSkill(321, "Wing of Storm Absorption PowUp", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3);
        this.CreateSkill(322, "Wing of Storm Defense PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3);
        this.CreateSkill(323, "Iron Defense", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, ragefighterClass: 3, growLancerClass: 3, crusaderClass: 3);
        this.CreateSkill(324, "Wing of Storm Attack PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3);
        this.CreateSkill(326, "Cyclone Strengthener", DamageType.Physical, damage: 4, distance: 2, abilityConsumption: 0, manaConsumption: 9, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 30), knightClass: 3, slayerClass: 3);
        this.CreateSkill(327, "Slash Strengthener", DamageType.Physical, damage: 40, distance: 2, abilityConsumption: 0, manaConsumption: 10, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 30), knightClass: 3, slayerClass: 3);
        this.CreateSkill(328, "Falling Slash Strengthener", DamageType.Physical, damage: 17, distance: 3, abilityConsumption: 0, manaConsumption: 9, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 30), knightClass: 3, crusaderClass: 3);
        this.CreateSkill(329, "Lunge Strengthener", DamageType.Physical, damage: 17, distance: 2, abilityConsumption: 0, manaConsumption: 9, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 30), knightClass: 3, growLancerClass: 3);
        this.CreateSkill(330, "Twisting Slash Strengthener", DamageType.Physical, damage: 40, distance: 2, abilityConsumption: 10, manaConsumption: 10, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 40), knightClass: 3, slayerClass: 3, crusaderClass: 3);
        this.CreateSkill(331, "Anger Blow Strengthener", DamageType.Physical, damage: 22, distance: 3, abilityConsumption: 22, manaConsumption: 25, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), knightClass: 3, slayerClass: 3, crusaderClass: 3);
        this.CreateSkill(332, "Twisting Slash Mastery", damage: 1, distance: 2, abilityConsumption: 20, manaConsumption: 22, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 40), knightClass: 3, crusaderClass: 3);
        this.CreateSkill(333, "Anger Blow Mastery", damage: 1, distance: 3, abilityConsumption: 30, manaConsumption: 50, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), knightClass: 3, crusaderClass: 3);
        this.CreateSkill(334, "Maximum Life Increase", damage: 9, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(335, "Weapon Mastery", damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, slayerClass: 3, gunCrusherClass: 3, illusionKnightClass: 3);
        this.CreateSkill(336, "Death Stab Strengthener", DamageType.Physical, damage: 22, distance: 4, abilityConsumption: 13, manaConsumption: 15, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), knightClass: 3, crusaderClass: 3);
        this.CreateSkill(338, "Maximum Mana Increase", damage: 9, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(339, "Death Stab Proficiency", damage: 22, distance: 4, abilityConsumption: 26, manaConsumption: 30, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), knightClass: 3);
        this.CreateSkill(340, "Strike of Destruction Proficiency", damage: 22, distance: 5, abilityConsumption: 24, manaConsumption: 30, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), knightClass: 3, crusaderClass: 3);
        this.CreateSkill(341, "Maximum AG Increase", damage: 8, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(342, "Death Stab Mastery", damage: 7, distance: 4, abilityConsumption: 26, manaConsumption: 30, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), knightClass: 3);
        this.CreateSkill(343, "Strike of Destruction Mastery", damage: 22, distance: 5, abilityConsumption: 24, manaConsumption: 30, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), knightClass: 3, crusaderClass: 3);
        this.CreateSkill(344, "Blood Storm", damage: 25, distance: 3, abilityConsumption: 29, manaConsumption: 87, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 200), knightClass: 3, magicGladiatorClass: 3, crusaderClass: 3);
        this.CreateSkill(345, "Combo Strengthener", damage: 7, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3);
        this.CreateSkill(346, "Blood Storm Strengthener", DamageType.Physical, damage: 5, distance: 3, abilityConsumption: 31, manaConsumption: 95, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 200), knightClass: 3, magicGladiatorClass: 3, crusaderClass: 3);
        this.CreateSkill(348, "Two-handed Sword Strengthener", DamageType.Physical, damage: 42, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, magicGladiatorClass: 3);
        this.CreateSkill(349, "One-handed Sword Strengthener", DamageType.Physical, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, magicGladiatorClass: 3);
        this.CreateSkill(350, "Mace Strengthener", DamageType.Physical, damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, crusaderClass: 3);
        this.CreateSkill(351, "Spear Strengthener", DamageType.Physical, damage: 41, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3);
        this.CreateSkill(352, "Two-handed Sword Mastery", damage: 41, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, magicGladiatorClass: 3);
        this.CreateSkill(353, "One-handed Sword Mastery", damage: 23, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, magicGladiatorClass: 3);
        this.CreateSkill(354, "Mace Mastery", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, crusaderClass: 3);
        this.CreateSkill(355, "Spear Mastery", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3);
        this.CreateSkill(356, "Swell Life Strengthener", damage: 7, distance: 0, abilityConsumption: 26, manaConsumption: 24, knightClass: 3, slayerClass: 3);
        this.CreateSkill(357, "Mana Reduction", damage: 18, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(358, "Monster Attack SD Increment", damage: 11, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(359, "Monster Attack Life Increment", damage: 6, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(360, "Swell Life Proficiency", damage: 7, distance: 0, abilityConsumption: 28, manaConsumption: 26, knightClass: 3, slayerClass: 3);
        this.CreateSkill(361, "Minimum Attack Power Increase", DamageType.Physical, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, magicGladiatorClass: 3, darkLordClass: 3, growLancerClass: 3, slayerClass: 3, gunCrusherClass: 3, illusionKnightClass: 3, crusaderClass: 3);
        this.CreateSkill(362, "Monster Attack Mana Increment", damage: 6, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(363, "Swell Life Mastery", damage: 7, distance: 0, abilityConsumption: 30, manaConsumption: 28, knightClass: 3);
        this.CreateSkill(364, "Maximum Attack Power Increase", DamageType.Physical, damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, magicGladiatorClass: 3, darkLordClass: 3, growLancerClass: 3, slayerClass: 3, gunCrusherClass: 3, illusionKnightClass: 3, crusaderClass: 3);
        this.CreateSkill(366, "Increases critical damage rate", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(367, "Restores all Mana", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(368, "Restores all HP", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(369, "Increases excellent damage rate", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(370, "Increases double damage rate", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(371, "Increases chance of ignore Def", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(372, "Restores all SD", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(373, "Increases triple damage rate", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, runeWizardClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(374, "Eternal Wings Absorption PowUp", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3);
        this.CreateSkill(375, "Eternal Wings Defense PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3);
        this.CreateSkill(377, "Eternal Wings Attack PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3);
        this.CreateSkill(378, "Flame Strengthener", DamageType.Wizardry, damage: 17, distance: 6, abilityConsumption: 0, manaConsumption: 55, energyRequirement: 160, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 100), wizardClass: 3, runeWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(379, "Lightning Strengthener", DamageType.Wizardry, damage: 40, distance: 6, abilityConsumption: 0, manaConsumption: 20, energyRequirement: 72, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 40), wizardClass: 3, runeWizardClass: 3, whiteWizardClass: 3, alchemistClass: 3);
        this.CreateSkill(380, "Expansion of Wizardry Power Up", DamageType.Wizardry, damage: 1, distance: 6, abilityConsumption: 55, manaConsumption: 220, energyRequirement: 1058, wizardClass: 3, runeWizardClass: 3, whiteWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(381, "Inferno Strengthener", DamageType.Wizardry, damage: 4, distance: 4, abilityConsumption: 0, manaConsumption: 220, energyRequirement: 724, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 90), wizardClass: 3, runeWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(382, "Blast Strengthener", DamageType.Wizardry, damage: 17, distance: 6, abilityConsumption: 0, manaConsumption: 165, energyRequirement: 500, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), wizardClass: 3, runeWizardClass: 3, whiteWizardClass: 3, alchemistClass: 3);
        this.CreateSkill(383, "Expansion of Wizardry Mastery", damage: 1, distance: 6, abilityConsumption: 55, manaConsumption: 220, energyRequirement: 1058, wizardClass: 3, runeWizardClass: 3, whiteWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(384, "Poison Strengthener", DamageType.Wizardry, damage: 40, distance: 6, abilityConsumption: 0, manaConsumption: 46, energyRequirement: 140, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 50), wizardClass: 3, runeWizardClass: 3, alchemistClass: 3);
        this.CreateSkill(385, "Evil Spirit Strengthener", DamageType.Wizardry, damage: 4, distance: 6, abilityConsumption: 0, manaConsumption: 108, energyRequirement: 220, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), wizardClass: 3, runeWizardClass: 3, alchemistClass: 3);
        this.CreateSkill(386, "Magic Mastery", DamageType.Wizardry, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, runeWizardClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(387, "Decay Strengthener", DamageType.Wizardry, damage: 17, distance: 6, abilityConsumption: 10, manaConsumption: 120, energyRequirement: 953, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 100), wizardClass: 3, runeWizardClass: 3, alchemistClass: 3);
        this.CreateSkill(388, "Hellfire Strengthener", DamageType.Wizardry, damage: 40, distance: 3, abilityConsumption: 0, manaConsumption: 176, energyRequirement: 260, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), wizardClass: 3, runeWizardClass: 3, alchemistClass: 3);
        this.CreateSkill(389, "Ice Strengthener", DamageType.Wizardry, damage: 40, distance: 6, abilityConsumption: 0, manaConsumption: 42, energyRequirement: 120, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), wizardClass: 3, runeWizardClass: 3, whiteWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(390, "Meteor Strengthener", DamageType.Wizardry, damage: 40, distance: 6, abilityConsumption: 0, manaConsumption: 13, energyRequirement: 104, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), wizardClass: 3, runeWizardClass: 3, alchemistClass: 3);
        this.CreateSkill(391, "Ice Storm Strengthener", DamageType.Wizardry, damage: 17, distance: 6, abilityConsumption: 5, manaConsumption: 110, energyRequirement: 849, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 90), wizardClass: 3, runeWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(392, "Nova Strengthener", DamageType.Wizardry, damage: 17, distance: 6, abilityConsumption: 49, manaConsumption: 198, energyRequirement: 1052, elementalModifier: (ElementalType)6, wizardClass: 3, runeWizardClass: 3, whiteWizardClass: 3, alchemistClass: 3);
        this.CreateSkill(393, "Ice Storm Mastery", damage: 22, distance: 6, abilityConsumption: 5, manaConsumption: 110, energyRequirement: 849, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 90), wizardClass: 3, runeWizardClass: 3, alchemistClass: 3);
        this.CreateSkill(394, "Meteor Mastery", damage: 1, distance: 6, abilityConsumption: 0, manaConsumption: 14, energyRequirement: 104, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), wizardClass: 3, runeWizardClass: 3, alchemistClass: 3);
        this.CreateSkill(395, "Nova Cast Strengthener", DamageType.Wizardry, damage: 22, distance: 6, abilityConsumption: 49, manaConsumption: 198, energyRequirement: 1052, elementalModifier: (ElementalType)6, wizardClass: 3, alchemistClass: 3);
        this.CreateSkill(397, "One-handed Staff Strengthener", DamageType.Wizardry, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, magicGladiatorClass: 3, whiteWizardClass: 3);
        this.CreateSkill(398, "Two-handed Staff Strengthener", DamageType.Wizardry, damage: 42, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, magicGladiatorClass: 3);
        this.CreateSkill(399, "Shield Strengthener", damage: 10, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, runeWizardClass: 3, lemuriaClass: 3);
        this.CreateSkill(400, "One-handed Staff Mastery", damage: 23, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, magicGladiatorClass: 3, whiteWizardClass: 3);
        this.CreateSkill(401, "Two-handed Staff Mastery", damage: 42, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, magicGladiatorClass: 3);
        this.CreateSkill(402, "Shield Mastery", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, runeWizardClass: 3, lemuriaClass: 3);
        this.CreateSkill(403, "Soul Barrier Strengthener", damage: 7, distance: 6, abilityConsumption: 24, manaConsumption: 77, energyRequirement: 408, wizardClass: 3);
        this.CreateSkill(404, "Soul Barrier Proficiency", damage: 10, distance: 6, abilityConsumption: 26, manaConsumption: 84, energyRequirement: 408, wizardClass: 3);
        this.CreateSkill(405, "Minimum Wizardry Increase", DamageType.Wizardry, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, magicGladiatorClass: 3, runeWizardClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(406, "Soul Barrier Mastery", damage: 7, distance: 6, abilityConsumption: 28, manaConsumption: 92, energyRequirement: 408, wizardClass: 3);
        this.CreateSkill(407, "Maximum Wizardry Increase", DamageType.Wizardry, damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, magicGladiatorClass: 3, runeWizardClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(409, "Illusion Wings Absorption PowUp", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(410, "Illusion Wings Defense PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(411, "Multi-Shot Strengthener", DamageType.Physical, damage: 22, distance: 6, abilityConsumption: 7, manaConsumption: 11, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), elfClass: 3);
        this.CreateSkill(412, "Illusion Wings Attack PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(413, "Heal Strengthener", DamageType.Wizardry, damage: 22, distance: 6, abilityConsumption: 0, manaConsumption: 22, energyRequirement: 52, elfClass: 3);
        this.CreateSkill(414, "Triple Shot Strengthener", DamageType.Physical, damage: 4, distance: 6, abilityConsumption: 0, manaConsumption: 5, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), elfClass: 3);
        this.CreateSkill(415, "Summoned Monster Power Up(1)", damage: 16, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(416, "Penetration Strengthener", DamageType.Physical, damage: 17, distance: 6, abilityConsumption: 11, manaConsumption: 10, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 200), elfClass: 3);
        this.CreateSkill(417, "Defense Increase Strengthener", DamageType.Wizardry, damage: 22, distance: 6, abilityConsumption: 0, manaConsumption: 33, energyRequirement: 72, elfClass: 3);
        this.CreateSkill(418, "Triple Shot Mastery", damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 9, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), elfClass: 3);
        this.CreateSkill(419, "Summoned Monster Power Up(2)", damage: 16, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(420, "Attack Increase Strengthener", DamageType.Wizardry, damage: 22, distance: 6, abilityConsumption: 0, manaConsumption: 44, energyRequirement: 92, elfClass: 3);
        this.CreateSkill(421, "Weapon Mastery", DamageType.Physical, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(422, "Attack Increase Mastery", DamageType.Wizardry, damage: 22, distance: 6, abilityConsumption: 0, manaConsumption: 48, energyRequirement: 92, elfClass: 3);
        this.CreateSkill(423, "Defense Increase Mastery", DamageType.Wizardry, damage: 22, distance: 6, abilityConsumption: 0, manaConsumption: 36, energyRequirement: 72, elfClass: 3);
        this.CreateSkill(424, "Ice Arrow Strengthener", DamageType.Physical, damage: 22, distance: 8, abilityConsumption: 18, manaConsumption: 15, dexterityRequirement: 646, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 150), elfClass: 3);
        this.CreateSkill(425, "Cure", damage: 0, distance: 6, abilityConsumption: 10, manaConsumption: 72, elfClass: 3, lemuriaClass: 3);
        this.CreateSkill(426, "Party Healing", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 12, manaConsumption: 266, energyRequirement: 20, elfClass: 3);
        this.CreateSkill(427, "Poison Arrow", damage: 27, distance: 6, abilityConsumption: 50, manaConsumption: 50, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), elfClass: 3);
        this.CreateSkill(428, "Summoned Monster Power Up(3)", damage: 16, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(429, "Party Healing Strengthener", DamageType.Wizardry, damage: 22, distance: 6, abilityConsumption: 13, manaConsumption: 272, energyRequirement: 20, elfClass: 3);
        this.CreateSkill(430, "Bless", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 18, manaConsumption: 108, energyRequirement: 20, elfClass: 3);
        this.CreateSkill(431, "Multi-Shot Mastery", damage: 1, distance: 6, abilityConsumption: 8, manaConsumption: 12, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), elfClass: 3);
        this.CreateSkill(432, "Summon Satyros", damage: 0, distance: 6, abilityConsumption: 52, manaConsumption: 525, energyRequirement: 300, elfClass: 3);
        this.CreateSkill(433, "Bless Strengthener", DamageType.Wizardry, damage: 10, distance: 6, abilityConsumption: 20, manaConsumption: 118, energyRequirement: 20, elfClass: 3);
        this.CreateSkill(434, "Poison Arrow Strengthener", DamageType.Physical, damage: 40, distance: 6, abilityConsumption: 50, manaConsumption: 50, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), elfClass: 3);
        this.CreateSkill(435, "Bow Strengthener", DamageType.Physical, damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(436, "Crossbow Strengthener", DamageType.Physical, damage: 4, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(437, "Shield Strengthener", damage: 10, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(438, "Bow Mastery", damage: 23, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(439, "Crossbow Mastery", damage: 5, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(440, "Shield Mastery", damage: 15, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(441, "Infinity Arrow Strengthener", damage: 1, distance: 0, abilityConsumption: 11, manaConsumption: 55, elfClass: 3);
        this.CreateSkill(442, "Minimum Attack Power Increase", DamageType.Physical, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(443, "Maximum Attack Power Increase", DamageType.Physical, damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(445, "Dimension Wings Absorb PowUp", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(446, "Dimension Wings Defense PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(447, "Dimension Wings Attack PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(451, "Fire Tome Mastery", damage: 7, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(452, "Earth Tome Mastery", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(453, "Wind Tome Mastery", damage: 7, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(454, "Sleep Strengthener", damage: 1, distance: 6, abilityConsumption: 7, manaConsumption: 30, energyRequirement: 180, summonerClass: 3);
        this.CreateSkill(455, "Chain Lightning Strengthener", DamageType.Wizardry, damage: 22, distance: 6, abilityConsumption: 0, manaConsumption: 103, energyRequirement: 245, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), summonerClass: 3);
        this.CreateSkill(456, "Lightning Shock Strengthener", DamageType.Wizardry, damage: 22, distance: 6, abilityConsumption: 10, manaConsumption: 125, energyRequirement: 823, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), summonerClass: 3);
        this.CreateSkill(457, "Magic Mastery", damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(458, "Drain Life Strengthener", DamageType.Wizardry, damage: 22, distance: 6, abilityConsumption: 0, manaConsumption: 57, energyRequirement: 150, elementalModifier: (ElementalType)6, summonerClass: 3);
        this.CreateSkill(459, "Weakness Strengthener", damage: 3, distance: 8, abilityConsumption: 15, manaConsumption: 50, energyRequirement: 663, summonerClass: 3);
        this.CreateSkill(460, "Innovation Strengthener", damage: 3, distance: 8, abilityConsumption: 15, manaConsumption: 50, energyRequirement: 663, summonerClass: 3);
        this.CreateSkill(461, "Blind", damage: 0, distance: 3, abilityConsumption: 25, manaConsumption: 115, energyRequirement: 20, summonerClass: 3);
        this.CreateSkill(462, "Drain Life Mastery", DamageType.Wizardry, damage: 9, distance: 6, abilityConsumption: 0, manaConsumption: 62, energyRequirement: 150, elementalModifier: (ElementalType)6, summonerClass: 3);
        this.CreateSkill(463, "Blind Strengthener", damage: 1, distance: 3, abilityConsumption: 27, manaConsumption: 126, energyRequirement: 20, summonerClass: 3);
        this.CreateSkill(465, "Stick Strengthener", DamageType.Wizardry, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(466, "Other World Tome Strengthener", DamageType.Curse, damage: 4, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(467, "Stick Mastery", damage: 5, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(468, "Other World Tome Mastery", damage: 23, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(469, "Berserker Strengthener", damage: 172, distance: 5, abilityConsumption: 82, manaConsumption: 165, energyRequirement: 300, summonerClass: 3);
        this.CreateSkill(470, "Berserker Proficiency", damage: 173, distance: 5, abilityConsumption: 90, manaConsumption: 181, energyRequirement: 300, summonerClass: 3);
        this.CreateSkill(471, "Minimum Wizardry/Curse Increase", damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(473, "Maximum Wizardry/Curse Increase", damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(475, "Wing of Ruin Absorption PowUp", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3);
        this.CreateSkill(476, "Wing of Ruin Defense PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3);
        this.CreateSkill(478, "Wing of Ruin Attack PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3);
        this.CreateSkill(479, "Cyclone Strengthener", DamageType.Physical, damage: 4, distance: 2, abilityConsumption: 0, manaConsumption: 9, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 30), magicGladiatorClass: 3);
        this.CreateSkill(480, "Lightning Strengthener", DamageType.Wizardry, damage: 40, distance: 6, abilityConsumption: 0, manaConsumption: 20, energyRequirement: 72, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 40), magicGladiatorClass: 3);
        this.CreateSkill(481, "Twisting Slash Strengthener", DamageType.Physical, damage: 40, distance: 2, abilityConsumption: 10, manaConsumption: 10, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 40), magicGladiatorClass: 3);
        this.CreateSkill(482, "Power Slash Strengthener", DamageType.Physical, damage: 17, distance: 5, abilityConsumption: 0, manaConsumption: 15, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), magicGladiatorClass: 3);
        this.CreateSkill(483, "Flame Strengthener", DamageType.Wizardry, damage: 17, distance: 6, abilityConsumption: 0, manaConsumption: 55, energyRequirement: 160, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 100), magicGladiatorClass: 3);
        this.CreateSkill(484, "Blast Strengthener", DamageType.Wizardry, damage: 17, distance: 6, abilityConsumption: 0, manaConsumption: 165, energyRequirement: 500, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), magicGladiatorClass: 3);
        this.CreateSkill(485, "Weapon Mastery", DamageType.Physical, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3);
        this.CreateSkill(486, "Inferno Strengthener", DamageType.Wizardry, damage: 4, distance: 4, abilityConsumption: 0, manaConsumption: 220, energyRequirement: 724, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 90), magicGladiatorClass: 3);
        this.CreateSkill(487, "Evil Spirit Strengthener", DamageType.Wizardry, damage: 4, distance: 6, abilityConsumption: 0, manaConsumption: 108, energyRequirement: 220, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), magicGladiatorClass: 3);
        this.CreateSkill(488, "Magic Mastery", DamageType.Wizardry, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3);
        this.CreateSkill(489, "Ice Strengthener", DamageType.Wizardry, damage: 40, distance: 6, abilityConsumption: 0, manaConsumption: 42, energyRequirement: 120, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), magicGladiatorClass: 3);
        this.CreateSkill(490, "Fire Slash Strengthener", DamageType.Physical, damage: 3, distance: 3, abilityConsumption: 12, manaConsumption: 15, strengthRequirement: 596, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), magicGladiatorClass: 3);
        this.CreateSkill(491, "Ice Mastery", damage: 1, distance: 6, abilityConsumption: 0, manaConsumption: 46, energyRequirement: 120, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), magicGladiatorClass: 3);
        this.CreateSkill(492, "Flame Strike Strengthener", DamageType.Physical, damage: 4, distance: 3, abilityConsumption: 37, manaConsumption: 30, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 200), magicGladiatorClass: 3);
        this.CreateSkill(493, "Fire Slash Mastery", damage: 7, distance: 3, abilityConsumption: 12, manaConsumption: 17, strengthRequirement: 596, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), magicGladiatorClass: 3);
        this.CreateSkill(494, "Flame Strike Mastery", damage: 7, distance: 3, abilityConsumption: 40, manaConsumption: 33, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 200), magicGladiatorClass: 3);
        this.CreateSkill(495, "Earth Prison", DamageType.Wizardry, damage: 26, distance: 3, abilityConsumption: 15, manaConsumption: 180, energyRequirement: 20, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), wizardClass: 3, magicGladiatorClass: 3, runeWizardClass: 3, whiteWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(496, "Gigantic Storm Strengthener", DamageType.Wizardry, damage: 3, distance: 6, abilityConsumption: 10, manaConsumption: 132, energyRequirement: 1058, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 250), magicGladiatorClass: 3);
        this.CreateSkill(497, "Earth Prison Strengthener", DamageType.Wizardry, damage: 4, distance: 3, abilityConsumption: 17, manaConsumption: 198, energyRequirement: 20, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 200), wizardClass: 3, magicGladiatorClass: 3, runeWizardClass: 3, whiteWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(504, "Emperor Cape Absorption PowUp", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3);
        this.CreateSkill(511, "Lord Dignity Strengthener", damage: 321, distance: 0, abilityConsumption: 65, manaConsumption: 65, energyRequirement: 102, leadershipRequirement: 300, darkLordClass: 3);
        this.CreateSkill(512, "Earth-Shake Strengthener", DamageType.Physical, damage: 4, distance: 10, abilityConsumption: 75, manaConsumption: 0, elementalModifier: (ElementalType)6, darkLordClass: 3);
        this.CreateSkill(514, "Fire Burst Mastery", damage: 1, distance: 6, abilityConsumption: 0, manaConsumption: 13, energyRequirement: 79, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 80), darkLordClass: 3);
        this.CreateSkill(527, "Scepter Strengthener", DamageType.Physical, damage: 305, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3);
        this.CreateSkill(528, "Shield Strengthener", damage: 10, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3);
        this.CreateSkill(529, "Scepter Strengthener: Spirit", damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3);
        this.CreateSkill(530, "Spirit Critical DMG Probability Increase", damage: 7, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3);
        this.CreateSkill(531, "Scepter Mastery", damage: 5, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3);
        this.CreateSkill(532, "Shield Mastery", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3);
        this.CreateSkill(533, "Command Attack Increase", damage: 20, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3);
        this.CreateSkill(534, "Spirit Excellent DMG Probability Increase", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3);
        this.CreateSkill(538, "Spirit Noble", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3);
        this.CreateSkill(539, "Spirit Lord", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3);
        this.CreateSkill(548, "Reigning Cloak Absorption PowUp", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(549, "Reigning Cloak Defense PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(550, "Reigning Cloak Attack PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(551, "Killing Blow Strengthener", DamageType.Physical, damage: 22, distance: 2, abilityConsumption: 0, manaConsumption: 10, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalVitality, 100), ragefighterClass: 3);
        this.CreateSkill(552, "Beast Uppercut Strengthener", DamageType.Physical, damage: 22, distance: 2, abilityConsumption: 0, manaConsumption: 10, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalVitality, 100), ragefighterClass: 3);
        this.CreateSkill(554, "Killing Blow Mastery", damage: 1, distance: 2, abilityConsumption: 0, manaConsumption: 10, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalVitality, 100), ragefighterClass: 3);
        this.CreateSkill(555, "Beast Uppercut Mastery", damage: 1, distance: 2, abilityConsumption: 0, manaConsumption: 10, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalVitality, 100), ragefighterClass: 3);
        this.CreateSkill(556, "Def Success Rate Strengthener", damage: 5, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(557, "Weapon Mastery", damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(558, "Chain Drive Strengthener", DamageType.Physical, damage: 22, distance: 4, abilityConsumption: 22, manaConsumption: 22, elementalModifier: (ElementalType)6, ragefighterClass: 3);
        this.CreateSkill(559, "Dark Side Strengthener", DamageType.Physical, damage: 22, distance: 5, abilityConsumption: 0, manaConsumption: 84, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), ragefighterClass: 3);
        this.CreateSkill(560, "Dragon Roar Strengthener", DamageType.Physical, damage: 22, distance: 3, abilityConsumption: 33, manaConsumption: 60, elementalModifier: (ElementalType)6, ragefighterClass: 3);
        this.CreateSkill(561, "Dragon Roar Mastery", DamageType.Physical, damage: 1, distance: 3, abilityConsumption: 36, manaConsumption: 66, elementalModifier: (ElementalType)6, ragefighterClass: 3);
        this.CreateSkill(562, "Chain Drive Mastery", DamageType.Physical, damage: 1, distance: 4, abilityConsumption: 22, manaConsumption: 24, elementalModifier: (ElementalType)6, ragefighterClass: 3);
        this.CreateSkill(563, "Dark Side Mastery", DamageType.Physical, damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 92, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), ragefighterClass: 3);
        this.CreateSkill(564, "Dragon Slasher Strengthener", damage: 22, distance: 4, abilityConsumption: 110, manaConsumption: 110, elementalModifier: (ElementalType)6, ragefighterClass: 3);
        this.CreateSkill(565, "Blood Howling", damage: 0, distance: 0, abilityConsumption: 200, manaConsumption: 100, ragefighterClass: 3);
        this.CreateSkill(566, "Dragon Slasher Mastery", damage: 38, distance: 4, abilityConsumption: 121, manaConsumption: 121, elementalModifier: (ElementalType)6, ragefighterClass: 3);
        this.CreateSkill(567, "Blood Howling Strengthener", damage: 38, distance: 0, abilityConsumption: 220, manaConsumption: 110, ragefighterClass: 3);
        this.CreateSkill(568, "Equipped Weapon Strengthener", DamageType.Physical, damage: 305, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(569, "Defense Success Rate Increase PowUp", damage: 22, distance: 7, abilityConsumption: 11, manaConsumption: 55, energyRequirement: 80, ragefighterClass: 3);
        this.CreateSkill(571, "Equipped Weapon Mastery", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(572, "DefSuccessRate Increase Mastery", damage: 22, distance: 7, abilityConsumption: 12, manaConsumption: 60, energyRequirement: 80, ragefighterClass: 3);
        this.CreateSkill(573, "Stamina Increase Strengthener", damage: 5, distance: 7, abilityConsumption: 11, manaConsumption: 55, energyRequirement: 132, ragefighterClass: 3);
        this.CreateSkill(574, "Defense Switch", damage: 20, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(578, "Durability Reduction (1)", damage: 37, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(579, "Increase PvP Defense Rate", damage: 29, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(580, "Increase Maximum SD", damage: 33, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(581, "Increase Mana Recovery Rate", damage: 7, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(582, "Increase Poison Resistance", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, elementalModifier: (ElementalType)6, ragefighterClass: 3);
        this.CreateSkill(583, "Durability Reduction (2)", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(584, "Increase SD Recovery Rate", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(585, "Increase HP Recovery Rate", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(586, "Increase Lightning Resistance", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, elementalModifier: (ElementalType)6, ragefighterClass: 3);
        this.CreateSkill(587, "Increases Defense", damage: 35, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(588, "Increases AG Recovery Rate", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(589, "Increase Ice Resistance", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(590, "Durability Reduction(3)", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(591, "Increase Defense Success Rate", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(592, "Cast Invincibility", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(593, "Increase Set Defense", damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(594, "Vengeance", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(595, "Increase Energy", damage: 36, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(596, "Stamina Increases", damage: 36, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(597, "Increase Agility", damage: 36, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(598, "Increase Strength", damage: 36, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(599, "Increase Attack Success Rate", damage: 13, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(600, "Increase Maximum HP", damage: 34, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(601, "Increase Maximum Mana", damage: 34, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(602, "Increase Maximum AG", damage: 37, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(603, "Increase PvP Attack Rate", damage: 31, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(604, "Decrease Mana", damage: 18, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(605, "Recover SD from Monster Kills", damage: 11, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(606, "Recover HP from Monster Kills", damage: 6, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(607, "Increase Minimum Attack Power", DamageType.Physical, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(608, "Recover Mana from Monster Kills", damage: 6, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(609, "Increase Maximum Attack Power", DamageType.Physical, damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(610, "Increases Critical DMG Chance", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(611, "Recover Mana Fully", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(612, "Recovers HP Fully", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(613, "Increase Excellent DMG Chance", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(614, "Increase Double Damage Chance", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(615, "Increase Ignore Def Chance", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(616, "Recovers SD Fully", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(617, "Increase Triple Damage Chance", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(618, "Spell of Protection", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 30, wizardClass: 1, knightClass: 1, elfClass: 1, magicGladiatorClass: 1, darkLordClass: 1, summonerClass: 1, ragefighterClass: 1, growLancerClass: 1, slayerClass: 1, whiteWizardClass: 1, lemuriaClass: 1, illusionKnightClass: 1, alchemistClass: 1, crusaderClass: 1);
        this.CreateSkill(619, "Spell of Restriction", damage: 0, distance: 3, abilityConsumption: 0, manaConsumption: 30, wizardClass: 1, knightClass: 1, elfClass: 1, magicGladiatorClass: 1, darkLordClass: 1, summonerClass: 1, ragefighterClass: 1, growLancerClass: 1, slayerClass: 1, whiteWizardClass: 1, lemuriaClass: 1, illusionKnightClass: 1, alchemistClass: 1, crusaderClass: 1);
        this.CreateSkill(620, "Spell of Pursuit", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 30, wizardClass: 1, knightClass: 1, elfClass: 1, magicGladiatorClass: 1, darkLordClass: 1, summonerClass: 1, ragefighterClass: 1, growLancerClass: 1, slayerClass: 1, whiteWizardClass: 1, lemuriaClass: 1, illusionKnightClass: 1, alchemistClass: 1, crusaderClass: 1);
        this.CreateSkill(621, "Shied-Burn", damage: 0, distance: 3, abilityConsumption: 0, manaConsumption: 30, wizardClass: 1, knightClass: 1, elfClass: 1, magicGladiatorClass: 1, darkLordClass: 1, summonerClass: 1, ragefighterClass: 1, growLancerClass: 1, slayerClass: 1, whiteWizardClass: 1, lemuriaClass: 1, illusionKnightClass: 1, alchemistClass: 1, crusaderClass: 1);
        this.CreateSkill(623, "Protection Shield", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, illusionKnightClass: 3, crusaderClass: 3);
        this.CreateSkill(624, "Weapon Block", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, gunCrusherClass: 3, illusionKnightClass: 3, crusaderClass: 3);
        this.CreateSkill(625, "Shield Block", damage: 39, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, illusionKnightClass: 3, crusaderClass: 3);
        this.CreateSkill(626, "Steel Armor", damage: 35, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, gunCrusherClass: 3, illusionKnightClass: 3, crusaderClass: 3);
        this.CreateSkill(627, "Strong Mind", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, illusionKnightClass: 3, crusaderClass: 3);
        this.CreateSkill(628, "Absorb Life", damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, gunCrusherClass: 3, illusionKnightClass: 3, crusaderClass: 3);
        this.CreateSkill(629, "Absorb Shield", damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, gunCrusherClass: 3, illusionKnightClass: 3, crusaderClass: 3);
        this.CreateSkill(630, "Battle Mind", DamageType.Physical, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, slayerClass: 3, illusionKnightClass: 3, crusaderClass: 3);
        this.CreateSkill(631, "Rush", damage: 178, distance: 7, abilityConsumption: 200, manaConsumption: 200, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), slayerClass: 3);
        this.CreateSkill(634, "Protection Shield", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, whiteWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(635, "Weapon Block", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, whiteWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(636, "Shield Block", damage: 39, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, whiteWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(637, "Steel Armor", damage: 35, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, whiteWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(638, "Strong Mind", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, whiteWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(639, "Absorb Life", damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, whiteWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(640, "SD Absorption", damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, whiteWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(641, "Grand Magic PowUp", DamageType.Wizardry, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, whiteWizardClass: 3, lemuriaClass: 3, alchemistClass: 3);
        this.CreateSkill(642, "Illusion", damage: 0, distance: 6, abilityConsumption: 300, manaConsumption: 1000, wizardClass: 3, whiteWizardClass: 3, alchemistClass: 3);
        this.CreateSkill(643, "Protection Shield", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(644, "Weapon Block", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(645, "Shield Block", damage: 39, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(646, "Steel Armor", damage: 35, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(647, "Strong Mind", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(648, "Absorb Life", damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(649, "SD Absorption", damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(650, "Marksman", DamageType.Physical, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, elfClass: 3);
        this.CreateSkill(651, "Shadow Step", damage: 0, distance: 5, abilityConsumption: 100, manaConsumption: 150, elfClass: 3);
        this.CreateSkill(652, "Evasion", damage: 0, distance: 0, abilityConsumption: 200, manaConsumption: 200, elfClass: 3);
        this.CreateSkill(653, "Protection Shield", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(654, "Weapon Block", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(655, "Shield Block", damage: 39, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(656, "Steel Armor", damage: 35, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(657, "Strong Mind", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(658, "Absorb Life", damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(659, "SD Absorption", damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(660, "Pain of Curse", DamageType.Physical, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, summonerClass: 3);
        this.CreateSkill(663, "Protection Shield", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3);
        this.CreateSkill(664, "Weapon Block", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3);
        this.CreateSkill(665, "Shield Block", damage: 39, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3);
        this.CreateSkill(666, "Steel Armor", damage: 35, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3);
        this.CreateSkill(667, "Strong Mind", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3);
        this.CreateSkill(668, "Absorb Life", damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3);
        this.CreateSkill(669, "SD Absorption", damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3);
        this.CreateSkill(671, "Weapon Block", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3);
        this.CreateSkill(674, "Strong Mind", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3);
        this.CreateSkill(675, "Absorb Life", damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3);
        this.CreateSkill(676, "SD Absorption", damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3);
        this.CreateSkill(677, "Protection Shield", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(678, "Weapon Block", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(679, "Shield Block", damage: 39, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(680, "Steel Armor", damage: 35, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(681, "Strong Mind", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(682, "Absorb Life", damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(683, "SD Absorption", damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, ragefighterClass: 3);
        this.CreateSkill(684, "Cloak of Transcendence Absorption PowUp", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3);
        this.CreateSkill(685, "Cloak of Transcendence Defense PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3);
        this.CreateSkill(686, "Cloak of Transcendence Attack PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3);
        this.CreateSkill(687, "Spin Step Strengthener", DamageType.Physical, damage: 3, distance: 2, abilityConsumption: 0, manaConsumption: 14, dexterityRequirement: 150, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, -1), growLancerClass: 3);
        this.CreateSkill(688, "Harsh Strike Strengthener", DamageType.Physical, damage: 4, distance: 3, abilityConsumption: 0, manaConsumption: 14, strengthRequirement: 150, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 30), growLancerClass: 3);
        this.CreateSkill(689, "Weapon Mastery", DamageType.Physical, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3);
        this.CreateSkill(690, "Spin Step Mastery", DamageType.Physical, damage: 4, distance: 2, abilityConsumption: 0, manaConsumption: 16, dexterityRequirement: 150, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, -1), growLancerClass: 3);
        this.CreateSkill(691, "Harsh Strike Mastery", damage: 0, distance: 3, abilityConsumption: 0, manaConsumption: 16, strengthRequirement: 150, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 30), growLancerClass: 3);
        this.CreateSkill(692, "Magic Pin Strengthener", DamageType.Physical, damage: 4, distance: 2, abilityConsumption: 10, manaConsumption: 17, dexterityRequirement: 200, strengthRequirement: 200, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), growLancerClass: 3);
        this.CreateSkill(693, "Obsidian Strengthener", damage: 18, distance: 0, abilityConsumption: 50, manaConsumption: 50, energyRequirement: 200, scalingStat1: (Stats.TotalStrength, -1), growLancerClass: 3);
        this.CreateSkill(695, "Magic Pin Mastery", damage: 0, distance: 3, abilityConsumption: 13, manaConsumption: 20, dexterityRequirement: 200, strengthRequirement: 200, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), growLancerClass: 3);
        this.CreateSkill(696, "Breche Strengthener", DamageType.Physical, damage: 4, distance: 5, abilityConsumption: 10, manaConsumption: 16, dexterityRequirement: 300, strengthRequirement: 300, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), growLancerClass: 3);
        this.CreateSkill(698, "Breche Mastery", damage: 0, distance: 6, abilityConsumption: 10, manaConsumption: 16, dexterityRequirement: 300, strengthRequirement: 300, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), growLancerClass: 3);
        this.CreateSkill(699, "Shining Peak Strengthener", DamageType.Physical, damage: 116, distance: 4, abilityConsumption: 7, manaConsumption: 10, strengthRequirement: 600, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 1), scalingStat2: (Stats.TotalStrength, 5), growLancerClass: 3);
        this.CreateSkill(700, "Burst", damage: 38, distance: 0, abilityConsumption: 50, manaConsumption: 200, growLancerClass: 3);
        this.CreateSkill(701, "Burst PowUp", damage: 23, distance: 0, abilityConsumption: 52, manaConsumption: 210, growLancerClass: 3);
        this.CreateSkill(702, "Lance PowUp", DamageType.Physical, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3);
        this.CreateSkill(703, "Circle Shield PowUp", damage: 10, distance: 0, abilityConsumption: 50, manaConsumption: 100, scalingStat1: (Stats.TotalAgility, -1), growLancerClass: 3);
        this.CreateSkill(704, "Shield PowUp", damage: 10, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3, crusaderClass: 3);
        this.CreateSkill(705, "Lance Mastery", DamageType.Physical, damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3);
        this.CreateSkill(706, "Circle Shield Mastery", damage: 23, distance: 0, abilityConsumption: 50, manaConsumption: 100, scalingStat1: (Stats.TotalAgility, -1), growLancerClass: 3);
        this.CreateSkill(707, "Shield Mastery", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3, crusaderClass: 3);
        this.CreateSkill(708, "Wrath Strengthener", damage: 23, distance: 0, abilityConsumption: 50, manaConsumption: 110, scalingStat1: (Stats.TotalAgility, -1), growLancerClass: 3);
        this.CreateSkill(709, "Wrath Proficiency", damage: 23, distance: 0, abilityConsumption: 60, manaConsumption: 130, scalingStat1: (Stats.TotalAgility, -1), growLancerClass: 3);
        this.CreateSkill(710, "Wrath Mastery", damage: 38, distance: 0, abilityConsumption: 80, manaConsumption: 150, scalingStat1: (Stats.TotalAgility, -1), growLancerClass: 3);
        this.CreateSkill(711, "Increases Retaliation DMG", damage: 23, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3);
        this.CreateSkill(712, "Increases Rage DMG", damage: 23, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3);
        this.CreateSkill(713, "Protection Shield", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3);
        this.CreateSkill(714, "Weapon Block", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3);
        this.CreateSkill(715, "Shield Block", damage: 39, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3);
        this.CreateSkill(716, "Steel Armor", damage: 35, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3);
        this.CreateSkill(717, "Strong Mind", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3);
        this.CreateSkill(718, "Absorb Life", damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3);
        this.CreateSkill(719, "SD Absorption", damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, growLancerClass: 3);
        this.CreateSkill(720, "Immune I", damage: 0, distance: 0, abilityConsumption: 200, manaConsumption: 300);
        this.CreateSkill(721, "Immune II", damage: 0, distance: 0, abilityConsumption: 200, manaConsumption: 300);
        this.CreateSkill(722, "Berserker I", damage: 0, distance: 0, abilityConsumption: 300, manaConsumption: 500);
        this.CreateSkill(723, "Fire Blow", DamageType.Physical, damage: 120, distance: 5, abilityConsumption: 17, manaConsumption: 17, strengthRequirement: 1090, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), knightClass: 3);
        this.CreateSkill(724, "Meteor Strike", DamageType.Wizardry, damage: 100, distance: 7, abilityConsumption: 17, manaConsumption: 92, energyRequirement: 1073, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 170), wizardClass: 3);
        this.CreateSkill(725, "Meteor Storm", DamageType.Wizardry, damage: 105, distance: 6, abilityConsumption: 20, manaConsumption: 105, energyRequirement: 1160, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), wizardClass: 3);
        this.CreateSkill(726, "Soul Seeker", DamageType.Wizardry, damage: 125, distance: 7, abilityConsumption: 16, manaConsumption: 90, energyRequirement: 1115, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 170), wizardClass: 3);
        this.CreateSkill(727, "Focus Shot", DamageType.Physical, damage: 110, distance: 7, abilityConsumption: 9, manaConsumption: 12, dexterityRequirement: 1302, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), elfClass: 3);
        this.CreateSkill(729, "Fire Beast", DamageType.Wizardry, damage: 100, distance: 7, abilityConsumption: 10, manaConsumption: 95, energyRequirement: 1220, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 170), summonerClass: 3);
        this.CreateSkill(730, "Aqua Beast", DamageType.Wizardry, damage: 100, distance: 7, abilityConsumption: 10, manaConsumption: 95, energyRequirement: 1220, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 170), summonerClass: 3);
        this.CreateSkill(731, "Ice Blood", DamageType.Physical, damage: 330, distance: 3, abilityConsumption: 14, manaConsumption: 19, strengthRequirement: 900, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 750), magicGladiatorClass: 3);
        this.CreateSkill(732, "Fire Blood", DamageType.Physical, damage: 330, distance: 3, abilityConsumption: 14, manaConsumption: 19, strengthRequirement: 900, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 750), magicGladiatorClass: 3);
        this.CreateSkill(733, "Dark Blast", DamageType.Wizardry, damage: 120, distance: 7, abilityConsumption: 17, manaConsumption: 92, energyRequirement: 1073, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 170), magicGladiatorClass: 3);
        this.CreateSkill(734, "Meteor Strike", DamageType.Wizardry, damage: 120, distance: 7, abilityConsumption: 17, manaConsumption: 92, energyRequirement: 1073, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 170), magicGladiatorClass: 3);
        this.CreateSkill(737, "Wind Soul", DamageType.Physical, damage: 130, distance: 6, abilityConsumption: 17, manaConsumption: 35, strengthRequirement: 717, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), darkLordClass: 3);
        this.CreateSkill(739, "Dark Phoenix Shot", DamageType.Physical, damage: 650, distance: 7, abilityConsumption: 10, manaConsumption: 30, dexterityRequirement: 987, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 170), ragefighterClass: 3);
        this.CreateSkill(740, "Archangel's will", damage: 0, distance: 0, abilityConsumption: 150, manaConsumption: 100, wizardClass: 1, knightClass: 1, elfClass: 1, magicGladiatorClass: 1, darkLordClass: 1, summonerClass: 1, ragefighterClass: 1, growLancerClass: 1, runeWizardClass: 1, slayerClass: 1, gunCrusherClass: 1, whiteWizardClass: 1, lemuriaClass: 1, illusionKnightClass: 1, alchemistClass: 1, crusaderClass: 1);
        this.CreateSkill(743, "Max HP Boost", damage: 43, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, ragefighterClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(744, "Enhance Phoenix Shot", DamageType.Physical, damage: 22, distance: 4, abilityConsumption: 0, manaConsumption: 30, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalVitality, 200), ragefighterClass: 3);
        this.CreateSkill(745, "Phoenix Shot Mastery", damage: 1, distance: 4, abilityConsumption: 0, manaConsumption: 30, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalVitality, 200), ragefighterClass: 3);
        this.CreateSkill(747, "Pentagram Elemental Defense Increase", damage: 163, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, ragefighterClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(748, "Magic Arrow Strengthener", DamageType.Wizardry, damage: 22, distance: 8, abilityConsumption: 0, manaConsumption: 25, energyRequirement: 60, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 500), runeWizardClass: 3);
        this.CreateSkill(749, "Magic Arrow Mastery", DamageType.Wizardry, damage: 22, distance: 8, abilityConsumption: 0, manaConsumption: 35, energyRequirement: 60, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 500), runeWizardClass: 3);
        this.CreateSkill(750, "Plasma Ball Strengthener", DamageType.Wizardry, damage: 22, distance: 8, abilityConsumption: 45, manaConsumption: 130, energyRequirement: 300, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), runeWizardClass: 3);
        this.CreateSkill(751, "Plasma Ball Mastery", DamageType.Wizardry, damage: 22, distance: 8, abilityConsumption: 55, manaConsumption: 135, energyRequirement: 300, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), runeWizardClass: 3);
        this.CreateSkill(752, "Rune Mace Strengthener", DamageType.Wizardry, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, runeWizardClass: 3);
        this.CreateSkill(753, "Rune Mace Mastery", DamageType.Wizardry, damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, runeWizardClass: 3);
        this.CreateSkill(754, "Wings of Disillusion Defense Strengthener", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, runeWizardClass: 3);
        this.CreateSkill(755, "Wings of Disillusion Attack Strengthener", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, runeWizardClass: 3);
        this.CreateSkill(756, "Shield Block", damage: 39, distance: 0, abilityConsumption: 0, manaConsumption: 0, runeWizardClass: 3);
        this.CreateSkill(758, "Protection Shield", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, runeWizardClass: 3);
        this.CreateSkill(759, "Steel Armor", damage: 35, distance: 0, abilityConsumption: 0, manaConsumption: 0, runeWizardClass: 3);
        this.CreateSkill(760, "Strong Mind", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, runeWizardClass: 3);
        this.CreateSkill(761, "Absorb Life", damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, runeWizardClass: 3);
        this.CreateSkill(762, "Absorb Shield", damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, runeWizardClass: 3);
        this.CreateSkill(763, "Grand Magic Power Up", DamageType.Wizardry, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, runeWizardClass: 3);
        this.CreateSkill(765, "Burst Strengthener", damage: 23, distance: 0, abilityConsumption: 0, manaConsumption: 60, runeWizardClass: 3);
        this.CreateSkill(766, "Burst Mastery", damage: 243, distance: 0, abilityConsumption: 0, manaConsumption: 80, runeWizardClass: 3);
        this.CreateSkill(768, "Haste Strengthener", damage: 23, distance: 0, abilityConsumption: 60, manaConsumption: 0, runeWizardClass: 3);
        this.CreateSkill(769, "Haste Mastery", damage: 243, distance: 0, abilityConsumption: 80, manaConsumption: 0, runeWizardClass: 3);
        this.CreateSkill(770, "Darkness Strengthener", damage: 174, distance: 5, abilityConsumption: 50, manaConsumption: 100, energyRequirement: 620, summonerClass: 3);
        this.CreateSkill(771, "Darkness Mastery", damage: 175, distance: 5, abilityConsumption: 50, manaConsumption: 100, energyRequirement: 620, summonerClass: 3);
        this.CreateSkill(772, "Greatness Mastery", damage: 177, distance: 8, abilityConsumption: 15, manaConsumption: 50, energyRequirement: 663, summonerClass: 3);
        this.CreateSkill(773, "Innovation Mastery", damage: 177, distance: 8, abilityConsumption: 15, manaConsumption: 50, energyRequirement: 663, summonerClass: 3);
        this.CreateSkill(774, "Explosion Strengthener", DamageType.Curse, damage: 3, distance: 4, abilityConsumption: 0, manaConsumption: 35, energyRequirement: 80, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), summonerClass: 3);
        this.CreateSkill(775, "Requiem Strengthener", DamageType.Curse, damage: 3, distance: 4, abilityConsumption: 4, manaConsumption: 60, energyRequirement: 140, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), summonerClass: 3);
        this.CreateSkill(776, "Pollution Strengthener", DamageType.Curse, damage: 3, distance: 4, abilityConsumption: 8, manaConsumption: 70, energyRequirement: 542, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), summonerClass: 3);
        this.CreateSkill(777, "Pollution Strengthener", DamageType.Curse, damage: 22, distance: 5, abilityConsumption: 8, manaConsumption: 70, energyRequirement: 542, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), summonerClass: 3);
        this.CreateSkill(778, "Pollution Mastery", DamageType.Curse, damage: 22, distance: 5, abilityConsumption: 8, manaConsumption: 70, energyRequirement: 542, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), summonerClass: 3);
        this.CreateSkill(779, "Sword Inertia Strengthener", DamageType.Physical, damage: 22, distance: 6, abilityConsumption: 5, manaConsumption: 7, dexterityRequirement: 100, strengthRequirement: 50, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 150), slayerClass: 3);
        this.CreateSkill(780, "Sword Inertia Mastery", DamageType.Physical, damage: 0, distance: 7, abilityConsumption: 7, manaConsumption: 10, dexterityRequirement: 100, strengthRequirement: 50, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 150), slayerClass: 3);
        this.CreateSkill(781, "Bat Flock Strengthener", DamageType.Physical, damage: 22, distance: 6, abilityConsumption: 9, manaConsumption: 25, dexterityRequirement: 380, strengthRequirement: 100, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 150), slayerClass: 3);
        this.CreateSkill(782, "Bat Flock Mastery", DamageType.Physical, damage: 23, distance: 6, abilityConsumption: 12, manaConsumption: 30, dexterityRequirement: 380, strengthRequirement: 100, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 150), slayerClass: 3);
        this.CreateSkill(783, "Short Sword Strengthener", DamageType.Physical, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, slayerClass: 3);
        this.CreateSkill(784, "Short Sword Mastery", damage: 23, distance: 0, abilityConsumption: 0, manaConsumption: 0, slayerClass: 3);
        this.CreateSkill(785, "Wings of Silence Defense Strengthener", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, slayerClass: 3);
        this.CreateSkill(786, "Wings of Silence Attack Strengthener", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, slayerClass: 3);
        this.CreateSkill(787, "Demolish Strengthener", damage: 23, distance: 0, abilityConsumption: 0, manaConsumption: 60, dexterityRequirement: 1450, slayerClass: 3);
        this.CreateSkill(788, "Demolish Mastery", damage: 214, distance: 0, abilityConsumption: 0, manaConsumption: 80, dexterityRequirement: 1450, slayerClass: 3);
        this.CreateSkill(789, "Weapon Mastery", damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, slayerClass: 3, illusionKnightClass: 3);
        this.CreateSkill(790, "Steel Armor", damage: 35, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3, slayerClass: 3);
        this.CreateSkill(791, "Weapon Block", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, slayerClass: 3, illusionKnightClass: 3, crusaderClass: 3);
        this.CreateSkill(792, "Life Absorption", damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3, slayerClass: 3);
        this.CreateSkill(793, "SD Absorption", damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, knightClass: 3, slayerClass: 3, illusionKnightClass: 3, crusaderClass: 3);
        this.CreateSkill(794, "Detection Strengthener", damage: 0, distance: 0, abilityConsumption: 100, manaConsumption: 100, dexterityRequirement: 800, slayerClass: 3);
        this.CreateSkill(801, "Sword's Fury Strengthener", damage: 226, distance: 0, abilityConsumption: 17, manaConsumption: 21, strengthRequirement: 1060, knightClass: 3, gunCrusherClass: 3);
        this.CreateSkill(802, "Sword's Fury Mastery", damage: 227, distance: 0, abilityConsumption: 17, manaConsumption: 21, strengthRequirement: 1060, knightClass: 3, gunCrusherClass: 3);
        this.CreateSkill(803, "Solid Protection Strengthener", damage: 228, distance: 0, abilityConsumption: 20, manaConsumption: 65, energyRequirement: 1052, knightClass: 3);
        this.CreateSkill(804, "Solid Protection Proficiency", damage: 229, distance: 0, abilityConsumption: 20, manaConsumption: 65, energyRequirement: 1052, knightClass: 3);
        this.CreateSkill(805, "Solid Protection Mastery", damage: 230, distance: 0, abilityConsumption: 20, manaConsumption: 65, energyRequirement: 1052, knightClass: 3);
        this.CreateSkill(806, "Solid Protection Mastery", damage: 231, distance: 0, abilityConsumption: 20, manaConsumption: 65, energyRequirement: 1052, knightClass: 3);
        this.CreateSkill(807, "Strike of Destruction Strengthener", DamageType.Physical, damage: 232, distance: 5, abilityConsumption: 24, manaConsumption: 30, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), knightClass: 3, crusaderClass: 3);
        this.CreateSkill(809, "Strike of Destruction Mastery", DamageType.Physical, damage: 234, distance: 5, abilityConsumption: 24, manaConsumption: 30, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), knightClass: 3, crusaderClass: 3);
        this.CreateSkill(810, "Strong Belief Strengthener", damage: 22, distance: 0, abilityConsumption: 25, manaConsumption: 80, energyRequirement: 1040, knightClass: 3);
        this.CreateSkill(811, "Tornado Strengthener", DamageType.Physical, damage: 40, distance: 2, abilityConsumption: 10, manaConsumption: 10, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 40), knightClass: 3, crusaderClass: 3);
        this.CreateSkill(812, "Anger Blow Strengthener", DamageType.Physical, damage: 22, distance: 3, abilityConsumption: 22, manaConsumption: 25, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), knightClass: 3, crusaderClass: 3);
        this.CreateSkill(813, "Rush", damage: 178, distance: 7, abilityConsumption: 200, manaConsumption: 200, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), knightClass: 3, crusaderClass: 3);
        this.CreateSkill(814, "Increase Energy Stat", damage: 245, distance: 0, abilityConsumption: 0, manaConsumption: 0, gunCrusherClass: 3);
        this.CreateSkill(815, "Increase Vitality Stat", damage: 246, distance: 0, abilityConsumption: 0, manaConsumption: 0, gunCrusherClass: 3);
        this.CreateSkill(816, "Increase Agility Stat", damage: 247, distance: 0, abilityConsumption: 0, manaConsumption: 0, gunCrusherClass: 3);
        this.CreateSkill(817, "Increase Strength Stat", damage: 248, distance: 0, abilityConsumption: 0, manaConsumption: 0, gunCrusherClass: 3);
        this.CreateSkill(818, "Wings of Hit Defense Strengthener", damage: 249, distance: 0, abilityConsumption: 0, manaConsumption: 0, gunCrusherClass: 3);
        this.CreateSkill(819, "Wings of Hit Attack Strengthener", damage: 250, distance: 0, abilityConsumption: 0, manaConsumption: 0, gunCrusherClass: 3);
        this.CreateSkill(820, "Dark Plasma Strengthener", DamageType.Wizardry, damage: 251, distance: 6, abilityConsumption: 15, manaConsumption: 22, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 3);
        this.CreateSkill(821, "Dark Plasma Proficiency", DamageType.Wizardry, damage: 252, distance: 6, abilityConsumption: 15, manaConsumption: 22, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 3);
        this.CreateSkill(822, "Dark Plasma Mastery", DamageType.Wizardry, damage: 253, distance: 6, abilityConsumption: 15, manaConsumption: 22, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 3);
        this.CreateSkill(823, "Ice Break Strengthener", DamageType.Wizardry, damage: 254, distance: 6, abilityConsumption: 8, manaConsumption: 17, energyRequirement: 295, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 3);
        this.CreateSkill(824, "Ice Break Mastery", DamageType.Wizardry, damage: 255, distance: 6, abilityConsumption: 8, manaConsumption: 17, energyRequirement: 295, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 3);
        this.CreateSkill(825, "Death Fire Strengthener", DamageType.Wizardry, damage: 256, distance: 6, abilityConsumption: 10, manaConsumption: 8, energyRequirement: 100, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 3);
        this.CreateSkill(826, "Death Fire Mastery", DamageType.Wizardry, damage: 257, distance: 7, abilityConsumption: 10, manaConsumption: 8, energyRequirement: 100, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 3);
        this.CreateSkill(828, "Fixed Fire Strengthener", damage: 285, distance: 0, abilityConsumption: 50, manaConsumption: 100, energyRequirement: 300, gunCrusherClass: 3);
        this.CreateSkill(829, "Fixed Fire Mastery", damage: 286, distance: 0, abilityConsumption: 50, manaConsumption: 100, energyRequirement: 300, gunCrusherClass: 3);
        this.CreateSkill(831, "Magic Gun Strengthener", damage: 262, distance: 0, abilityConsumption: 0, manaConsumption: 0, gunCrusherClass: 3);
        this.CreateSkill(832, "Magic Gun Mastery", damage: 263, distance: 0, abilityConsumption: 0, manaConsumption: 0, gunCrusherClass: 3);
        this.CreateSkill(835, "Death Ice Strengthener", DamageType.Wizardry, damage: 269, distance: 6, abilityConsumption: 10, manaConsumption: 8, energyRequirement: 100, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 3);
        this.CreateSkill(836, "Death Ice Mastery", DamageType.Wizardry, damage: 270, distance: 7, abilityConsumption: 10, manaConsumption: 8, energyRequirement: 100, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 3);
        this.CreateSkill(837, "Cloak of Brilliance Defense PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, whiteWizardClass: 3);
        this.CreateSkill(838, "Cloak of Brilliance Attack PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, whiteWizardClass: 3);
        this.CreateSkill(839, "Steel Armor", damage: 35, distance: 0, abilityConsumption: 0, manaConsumption: 0, whiteWizardClass: 3);
        this.CreateSkill(840, "Eternal Wings Defense Strengthener", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, lemuriaClass: 3);
        this.CreateSkill(841, "Eternal Wings Attack Strengthener", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, lemuriaClass: 3);
        this.CreateSkill(842, "Steel Armor", damage: 35, distance: 0, abilityConsumption: 0, manaConsumption: 0, lemuriaClass: 3);
        this.CreateSkill(843, "Shining Bird Strengthener", DamageType.Wizardry, damage: 22, distance: 6, abilityConsumption: 0, manaConsumption: 20, energyRequirement: 70, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), whiteWizardClass: 3);
        this.CreateSkill(844, "Shining Bird Mastery", DamageType.Wizardry, damage: 0, distance: 7, abilityConsumption: 0, manaConsumption: 30, energyRequirement: 70, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), whiteWizardClass: 3);
        this.CreateSkill(845, "Magic Mastery", DamageType.Wizardry, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, whiteWizardClass: 3);
        this.CreateSkill(846, "Dragon Violent Strengthener", DamageType.Wizardry, damage: 22, distance: 6, abilityConsumption: 0, manaConsumption: 50, energyRequirement: 680, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), whiteWizardClass: 3);
        this.CreateSkill(847, "Dragon Violent Mastery", DamageType.Wizardry, damage: 0, distance: 7, abilityConsumption: 0, manaConsumption: 70, energyRequirement: 680, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), whiteWizardClass: 3);
        this.CreateSkill(848, "Marvel Burst Strengthener", DamageType.Wizardry, damage: 4, distance: 6, abilityConsumption: 0, manaConsumption: 20, energyRequirement: 104, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), lemuriaClass: 3);
        this.CreateSkill(849, "Marvel Burst Mastery", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 30, energyRequirement: 104, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), lemuriaClass: 3);
        this.CreateSkill(850, "Magic Mastery", DamageType.Wizardry, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, lemuriaClass: 3);
        this.CreateSkill(851, "Beginner Defense Improvement Strengthener", DamageType.Wizardry, damage: 271, distance: 6, abilityConsumption: 0, manaConsumption: 33, energyRequirement: 72, lemuriaClass: 3);
        this.CreateSkill(852, "Beginner Defense Improvement Mastery", DamageType.Wizardry, damage: 271, distance: 6, abilityConsumption: 0, manaConsumption: 36, energyRequirement: 72, lemuriaClass: 3);
        this.CreateSkill(853, "Beginner Attack Power Improvement Strengthener", DamageType.Wizardry, damage: 271, distance: 6, abilityConsumption: 0, manaConsumption: 44, energyRequirement: 92, lemuriaClass: 3);
        this.CreateSkill(854, "Beginner Attack Improvement Mastery", DamageType.Wizardry, damage: 271, distance: 6, abilityConsumption: 0, manaConsumption: 48, energyRequirement: 92, lemuriaClass: 3);
        this.CreateSkill(855, "Unleash Marvel Strengthener", DamageType.Wizardry, damage: 22, distance: 6, abilityConsumption: 0, manaConsumption: 65, energyRequirement: 700, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), lemuriaClass: 3);
        this.CreateSkill(856, "Unleash Marvel Mastery", DamageType.Wizardry, damage: 0, distance: 7, abilityConsumption: 0, manaConsumption: 70, energyRequirement: 700, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), lemuriaClass: 3);
        this.CreateSkill(857, "Beginner Bless Strengthener", DamageType.Wizardry, damage: 272, distance: 6, abilityConsumption: 20, manaConsumption: 118, energyRequirement: 150, lemuriaClass: 3);
        this.CreateSkill(858, "Intensive Care Strengthener", DamageType.Wizardry, damage: 271, distance: 6, abilityConsumption: 0, manaConsumption: 22, energyRequirement: 52, lemuriaClass: 3);
        this.CreateSkill(859, "Magic Book Strengthener", DamageType.Wizardry, damage: 23, distance: 0, abilityConsumption: 0, manaConsumption: 0, whiteWizardClass: 3);
        this.CreateSkill(860, "Magic Book Mastery", DamageType.Wizardry, damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, whiteWizardClass: 3);
        this.CreateSkill(861, "Reflection Barrier Strengthener", damage: 7, distance: 6, abilityConsumption: 24, manaConsumption: 77, energyRequirement: 408, whiteWizardClass: 3);
        this.CreateSkill(862, "Reflection Barrier Skills", damage: 10, distance: 6, abilityConsumption: 26, manaConsumption: 84, energyRequirement: 408, whiteWizardClass: 3);
        this.CreateSkill(863, "Reflection Barrier Mastery", damage: 7, distance: 6, abilityConsumption: 28, manaConsumption: 92, energyRequirement: 408, whiteWizardClass: 3);
        this.CreateSkill(864, "Orb Strengthener", DamageType.Wizardry, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, lemuriaClass: 3);
        this.CreateSkill(865, "Spiral Charge Strengthener", damage: 275, distance: 0, abilityConsumption: 17, manaConsumption: 21, strengthRequirement: 900, magicGladiatorClass: 3);
        this.CreateSkill(866, "Spiral Charge Mastery", damage: 278, distance: 0, abilityConsumption: 17, manaConsumption: 21, strengthRequirement: 900, magicGladiatorClass: 3);
        this.CreateSkill(867, "Crusher Charge Strengthener", damage: 276, distance: 0, abilityConsumption: 17, manaConsumption: 21, strengthRequirement: 900, magicGladiatorClass: 3);
        this.CreateSkill(868, "Crusher Charge Mastery", damage: 279, distance: 0, abilityConsumption: 17, manaConsumption: 21, strengthRequirement: 900, magicGladiatorClass: 3);
        this.CreateSkill(869, "Elemental Charge Strengthener", damage: 277, distance: 0, abilityConsumption: 20, manaConsumption: 65, energyRequirement: 1073, magicGladiatorClass: 3);
        this.CreateSkill(870, "Elemental Charge Mastery", damage: 280, distance: 0, abilityConsumption: 20, manaConsumption: 65, energyRequirement: 1073, magicGladiatorClass: 3);
        this.CreateSkill(871, "Two-handed Sword Strengthener", DamageType.Physical, damage: 281, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3);
        this.CreateSkill(872, "Two-handed Staff Strengthener", damage: 282, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3);
        this.CreateSkill(873, "Two-handed Sword Mastery", DamageType.Physical, damage: 283, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3);
        this.CreateSkill(874, "Two-handed Staff Mastery", damage: 284, distance: 0, abilityConsumption: 0, manaConsumption: 0, magicGladiatorClass: 3);
        this.CreateSkill(875, "Orb Mastery", DamageType.Wizardry, damage: 23, distance: 0, abilityConsumption: 0, manaConsumption: 0, lemuriaClass: 3);
        this.CreateSkill(876, "Holy Bolt Strengthener", DamageType.Wizardry, damage: 41, distance: 6, abilityConsumption: 0, manaConsumption: 20, energyRequirement: 1200, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), elfClass: 3);
        this.CreateSkill(877, "Charge Slash Strengthener", DamageType.Physical, damage: 294, distance: 6, abilityConsumption: 0, manaConsumption: 3, dexterityRequirement: 80, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), illusionKnightClass: 3);
        this.CreateSkill(878, "Charge Slash Mastery", DamageType.Physical, damage: 295, distance: 7, abilityConsumption: 0, manaConsumption: 3, dexterityRequirement: 80, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), illusionKnightClass: 3);
        this.CreateSkill(879, "Wind Glaive Strengthener", DamageType.Physical, damage: 296, distance: 5, abilityConsumption: 0, manaConsumption: 6, dexterityRequirement: 650, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 80), illusionKnightClass: 3);
        this.CreateSkill(880, "Wind Glaive Mastery", DamageType.Physical, damage: 297, distance: 6, abilityConsumption: 0, manaConsumption: 6, dexterityRequirement: 650, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 80), illusionKnightClass: 3);
        this.CreateSkill(881, "Blade Storm Strengthener", DamageType.Physical, damage: 298, distance: 6, abilityConsumption: 10, manaConsumption: 10, dexterityRequirement: 1150, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 50), illusionKnightClass: 3);
        this.CreateSkill(882, "Blade Storm Mastery", DamageType.Physical, damage: 299, distance: 6, abilityConsumption: 10, manaConsumption: 10, dexterityRequirement: 1150, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 50), illusionKnightClass: 3);
        this.CreateSkill(883, "Illusion Avatar Strengthener", DamageType.Physical, damage: 300, distance: 6, abilityConsumption: 0, manaConsumption: 40, dexterityRequirement: 900, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 80), illusionKnightClass: 3);
        this.CreateSkill(884, "Illusion Avatar Mastery", DamageType.Physical, damage: 301, distance: 6, abilityConsumption: 0, manaConsumption: 40, dexterityRequirement: 900, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 80), illusionKnightClass: 3);
        this.CreateSkill(885, "Illusion Blade Strengthener", damage: 302, distance: 0, abilityConsumption: 0, manaConsumption: 40, illusionKnightClass: 3);
        this.CreateSkill(886, "Illusion Blade Mastery", damage: 303, distance: 0, abilityConsumption: 0, manaConsumption: 40, illusionKnightClass: 3);
        this.CreateSkill(887, "Illusion Blade Mastery", damage: 304, distance: 0, abilityConsumption: 0, manaConsumption: 40, illusionKnightClass: 3);
        this.CreateSkill(888, "Stop Weapon", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, illusionKnightClass: 3);
        this.CreateSkill(889, "Steel Armor", damage: 35, distance: 0, abilityConsumption: 0, manaConsumption: 0, illusionKnightClass: 3);
        this.CreateSkill(890, "Reinforcement of Cloak of Death Defense", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, illusionKnightClass: 3);
        this.CreateSkill(891, "Reinforcement of Cloak of Death Attack", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, illusionKnightClass: 3);
        this.CreateSkill(892, "Blade Strengthener", DamageType.Physical, damage: 305, distance: 0, abilityConsumption: 0, manaConsumption: 0, illusionKnightClass: 3);
        this.CreateSkill(893, "Blade Mastery", damage: 306, distance: 0, abilityConsumption: 0, manaConsumption: 0, illusionKnightClass: 3);
        this.CreateSkill(894, "Oversting Strengthener", DamageType.Physical, damage: 313, distance: 6, abilityConsumption: 12, manaConsumption: 25, dexterityRequirement: 1470, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 80), growLancerClass: 3);
        this.CreateSkill(895, "Wrath Strengthener", damage: 312, distance: 0, abilityConsumption: 40, manaConsumption: 50, dexterityRequirement: 200, strengthRequirement: 200, scalingStat1: (Stats.TotalAgility, -1), growLancerClass: 3);
        this.CreateSkill(896, "Wild Breath Strengthener", DamageType.Physical, damage: 315, distance: 6, abilityConsumption: 18, manaConsumption: 35, strengthRequirement: 1020, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), growLancerClass: 3);
        this.CreateSkill(897, "Reinforcement of Wing of the Guardian Defense", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, alchemistClass: 3);
        this.CreateSkill(898, "Reinforcement of Wing of the Guardian Attack", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, alchemistClass: 3);
        this.CreateSkill(899, "Steel Armor", damage: 35, distance: 0, abilityConsumption: 0, manaConsumption: 0, alchemistClass: 3);
        this.CreateSkill(900, "Angel Homunculus Strengthener", DamageType.Wizardry, damage: 22, distance: 6, abilityConsumption: 0, manaConsumption: 20, energyRequirement: 70, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), alchemistClass: 3);
        this.CreateSkill(901, "Angel Homunculus Mastery", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 30, energyRequirement: 70, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), alchemistClass: 3);
        this.CreateSkill(902, "Magic Mastery", DamageType.Wizardry, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, alchemistClass: 3);
        this.CreateSkill(903, "Ignition Bomber Strengthener", DamageType.Wizardry, damage: 22, distance: 6, abilityConsumption: 0, manaConsumption: 50, energyRequirement: 680, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), alchemistClass: 3);
        this.CreateSkill(904, "Ignition Bomber Mastery", DamageType.Wizardry, damage: 0, distance: 7, abilityConsumption: 5, manaConsumption: 70, energyRequirement: 680, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), alchemistClass: 3);
        this.CreateSkill(905, "Confusion Stone Strengthener", damage: 0, distance: 6, abilityConsumption: 50, manaConsumption: 80, energyRequirement: 850, alchemistClass: 3);
        this.CreateSkill(906, "Confusion Stone Mastery", damage: 23, distance: 6, abilityConsumption: 55, manaConsumption: 100, energyRequirement: 850, alchemistClass: 3);
        this.CreateSkill(907, "Wand Strengthen", DamageType.Wizardry, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, alchemistClass: 3);
        this.CreateSkill(909, "Elixir Strengthen", DamageType.Wizardry, damage: 23, distance: 0, abilityConsumption: 0, manaConsumption: 0, alchemistClass: 3);
        this.CreateSkill(910, "Elixir Mastery", DamageType.Wizardry, damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, alchemistClass: 3);
        this.CreateSkill(912, "Crown Force Strengthen", DamageType.Wizardry, damage: 318, distance: 0, abilityConsumption: 20, manaConsumption: 80, energyRequirement: 300, leadershipRequirement: 400, darkLordClass: 3);
        this.CreateSkill(913, "Divine Force Strengthen", DamageType.Wizardry, damage: 319, distance: 0, abilityConsumption: 0, manaConsumption: 100, leadershipRequirement: 500, darkLordClass: 3);
        this.CreateSkill(914, "Divine Aura Strengthen", DamageType.Wizardry, damage: 320, distance: 0, abilityConsumption: 0, manaConsumption: 100, leadershipRequirement: 500, darkLordClass: 3);
        this.CreateSkill(915, "Battle Glory Strengthen", damage: 323, distance: 0, abilityConsumption: 40, manaConsumption: 50, leadershipRequirement: 200, strengthRequirement: 200, darkLordClass: 3);
        this.CreateSkill(916, "Runic Spear Strengthen", DamageType.Wizardry, damage: 22, distance: 7, abilityConsumption: 0, manaConsumption: 35, energyRequirement: 600, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 100), runeWizardClass: 3);
        this.CreateSkill(917, "Cloak of Desire Defense Enhancement", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, crusaderClass: 3);
        this.CreateSkill(918, "Cloak of Desire Attack Enhancement", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, crusaderClass: 3);
        this.CreateSkill(919, "Divine Fall Reinforcement", DamageType.Physical, damage: 326, distance: 6, abilityConsumption: 0, manaConsumption: 10, dexterityRequirement: 29, strengthRequirement: 60, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 45), crusaderClass: 3);
        this.CreateSkill(920, "Divine Fall Mastery", DamageType.Physical, damage: 327, distance: 6, abilityConsumption: 0, manaConsumption: 13, dexterityRequirement: 29, strengthRequirement: 60, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 45), crusaderClass: 3);
        this.CreateSkill(921, "Holy Sweep Reinforcement", DamageType.Physical, damage: 324, distance: 5, abilityConsumption: 2, manaConsumption: 14, dexterityRequirement: 168, strengthRequirement: 420, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 10), crusaderClass: 3);
        this.CreateSkill(923, "Sacred Impact Enhancement", DamageType.Physical, damage: 328, distance: 7, abilityConsumption: 16, manaConsumption: 22, dexterityRequirement: 418, strengthRequirement: 1045, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 50), crusaderClass: 3);
        this.CreateSkill(925, "Weapon Mastery", damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, crusaderClass: 3);
        this.CreateSkill(926, "Hammer reinforcement", DamageType.Physical, damage: 330, distance: 0, abilityConsumption: 0, manaConsumption: 0, crusaderClass: 3);
        this.CreateSkill(927, "Hammer Mastery", damage: 331, distance: 0, abilityConsumption: 0, manaConsumption: 0, crusaderClass: 3);
        this.CreateSkill(928, "Lugard's Guardian: Strengthening Punishment", damage: 332, distance: 8, abilityConsumption: 30, manaConsumption: 210, energyRequirement: 200, crusaderClass: 3);
        this.CreateSkill(1001, "Increase Skill DMG", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1002, "DMG Count +1", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1003, "DMG Count +2", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1004, "DMG Count +3", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1005, "DMG Count +4", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1006, "DMG Count +2", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1007, "Increase Additional DMG Rate", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1008, "Increase Attack Speed", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1009, "Increase Range", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1010, "Add Splash DMG", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1011, "Increase Skill Range", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1012, "Increase Target", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1013, "Buff Synergy Effect", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1014, "Buff Synergy Effect", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1015, "Increase Skill Duration", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1016, "Iron Defense (Learned)", damage: 3, distance: 0, abilityConsumption: 31, manaConsumption: 70, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1017, "Enhance Iron Defense", damage: 3, distance: 0, abilityConsumption: 31, manaConsumption: 70, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1018, "Cooldown Time Reduction", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1019, "Remove Cooldown Time", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1020, "Weapon DMG Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1021, "Weapon Magic DMG Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1022, "Add Penetration Effect", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1023, "Add Arrow Missile", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1024, "Increase chance to create Poison Magic Circles", damage: 151, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1025, "Increase chance to create Chilling Magic Circle", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1026, "Increase chance to create Bleeding Magic Circle", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1028, "Poison Damage Enhancement", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1029, "Chilling Effect Enhancement", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1030, "Bleeding Damage Enhancement", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1032, "Increase Poison Magic Circle Duration", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1033, "Increase Chilling Magic Circle Duration", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1034, "Increase Bleeding Magic Circle Duration", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1036, "Addiction Damage Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1037, "Freezing Damage Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1038, "Hemorrhage Damage Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1040, "Increase chance to enhance to Addiction Magic Circle", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1041, "Increase chance to enhance to Freezing Magic Circle", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1042, "Increase chance to enhance to Hemorrhage Magic Circle", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1046, "Additional chance to enhance to Poison Magic Circles", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1047, "Additional chance to enhance to Freezing Magic Circle", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1048, "Additional chance to enhance to Hemorrhage Magic Circle", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1050, "Additional Poison Damage Enhancement", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1051, "Additional Chilling Effect Enhancement", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1052, "Additional Bleeding Damage Enhancement", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1054, "Increase Addiction Magic Circle Duration", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1055, "Increase Freeze Magic Circle Duration", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1056, "Increase Hemorrhage Magic Circle Duration", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1058, "Additional Addiction Damage Enhancement", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1059, "Additional Freezing Effect Enhancement", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1060, "Additional Hemorrhage Effect Enhancement", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1062, "Upgrade Poisoning (Addiction)", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1063, "Upgrade Chilling (Freezing)", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1064, "Upgrade Bleeding (Hemorrhage)", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1069, "Anger Blow Enhancement Skill", DamageType.Physical, damage: 60, distance: 3, abilityConsumption: 22, manaConsumption: 25, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), knightClass: 4, gunCrusherClass: 4);
        this.CreateSkill(1071, "Death Stab Enhancement Skill", damage: 22, distance: 4, abilityConsumption: 18, manaConsumption: 17, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), knightClass: 4, gunCrusherClass: 4);
        this.CreateSkill(1072, "Fire Blow Enhancement Skill", DamageType.Physical, damage: 120, distance: 5, abilityConsumption: 17, manaConsumption: 17, strengthRequirement: 100, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), knightClass: 4, gunCrusherClass: 4);
        this.CreateSkill(1075, "Meteor Strike Enhancement Skill", DamageType.Wizardry, damage: 100, distance: 7, abilityConsumption: 17, manaConsumption: 92, energyRequirement: 1073, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 170), wizardClass: 4);
        this.CreateSkill(1076, "Meteor Storm Enhancement Skill", DamageType.Wizardry, damage: 105, distance: 6, abilityConsumption: 20, manaConsumption: 105, energyRequirement: 1160, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), wizardClass: 4);
        this.CreateSkill(1078, "Evil Spirit Enhancement Skill", DamageType.Wizardry, damage: 45, distance: 6, abilityConsumption: 7, manaConsumption: 108, energyRequirement: 220, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), wizardClass: 4, magicGladiatorClass: 4);
        this.CreateSkill(1081, "Triple Shot Enhancement Skill", damage: 0, distance: 6, abilityConsumption: 4, manaConsumption: 9, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), elfClass: 4);
        this.CreateSkill(1083, "Multi-Shot Enhancement Skill", DamageType.Physical, damage: 22, distance: 6, abilityConsumption: 7, manaConsumption: 11, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), elfClass: 4);
        this.CreateSkill(1085, "Focus Shot Enhancement Skill", DamageType.Physical, damage: 110, distance: 7, abilityConsumption: 9, manaConsumption: 12, dexterityRequirement: 1302, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), elfClass: 4);
        this.CreateSkill(1087, "Gigantic Storm Enhancement Skill", DamageType.Wizardry, damage: 110, distance: 6, abilityConsumption: 11, manaConsumption: 110, energyRequirement: 1058, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 250), magicGladiatorClass: 4);
        this.CreateSkill(1088, "Evil Spirit Enhancement Skill", DamageType.Wizardry, damage: 45, distance: 6, abilityConsumption: 7, manaConsumption: 108, energyRequirement: 220, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), magicGladiatorClass: 4);
        this.CreateSkill(1089, "Dark Blast Enhancement Skill", DamageType.Wizardry, damage: 120, distance: 7, abilityConsumption: 17, manaConsumption: 92, energyRequirement: 1073, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 170), magicGladiatorClass: 4);
        this.CreateSkill(1092, "Fire Slash Enhancement Skill", DamageType.Physical, damage: 80, distance: 3, abilityConsumption: 12, manaConsumption: 15, strengthRequirement: 596, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 150), magicGladiatorClass: 4);
        this.CreateSkill(1094, "Fire Blood Enhancement Skill", DamageType.Physical, damage: 330, distance: 3, abilityConsumption: 14, manaConsumption: 19, strengthRequirement: 900, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 750), magicGladiatorClass: 4);
        this.CreateSkill(1095, "Ice Blood Enhancement Skill", DamageType.Physical, damage: 330, distance: 3, abilityConsumption: 14, manaConsumption: 19, strengthRequirement: 900, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 750), magicGladiatorClass: 4);
        this.CreateSkill(1096, "Fire Burst Enhancement Skill", DamageType.Physical, damage: 150, distance: 6, abilityConsumption: 8, manaConsumption: 20, energyRequirement: 79, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 80), darkLordClass: 4);
        this.CreateSkill(1098, "Chaotic Diseier Enhancement Skill", DamageType.Physical, damage: 220, distance: 6, abilityConsumption: 11, manaConsumption: 22, energyRequirement: 84, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 80), darkLordClass: 4);
        this.CreateSkill(1099, "Wind Soul Enhancement Skill", DamageType.Physical, damage: 130, distance: 6, abilityConsumption: 19, manaConsumption: 38, strengthRequirement: 717, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), darkLordClass: 4);
        this.CreateSkill(1102, "Fire Beast Enhancement Skill", DamageType.Wizardry, damage: 100, distance: 7, abilityConsumption: 11, manaConsumption: 95, energyRequirement: 1220, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 170), summonerClass: 4);
        this.CreateSkill(1103, "Lightning Shock Enhancement Skill", DamageType.Wizardry, damage: 95, distance: 6, abilityConsumption: 10, manaConsumption: 105, energyRequirement: 823, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), summonerClass: 4);
        this.CreateSkill(1105, "Aqua Beast Enhancement Skill", DamageType.Wizardry, damage: 100, distance: 7, abilityConsumption: 11, manaConsumption: 95, energyRequirement: 1220, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 170), summonerClass: 4);
        this.CreateSkill(1111, "Dragon Roar Enhancement Skill", DamageType.Physical, damage: 0, distance: 3, abilityConsumption: 17, manaConsumption: 42, elementalModifier: (ElementalType)6, ragefighterClass: 4);
        this.CreateSkill(1112, "Chain Drive Enhancement Skill", DamageType.Physical, damage: 0, distance: 4, abilityConsumption: 18, manaConsumption: 22, elementalModifier: (ElementalType)6, ragefighterClass: 4);
        this.CreateSkill(1113, "Dark Side Enhancement Skill", DamageType.Physical, damage: 0, distance: 6, abilityConsumption: 7, manaConsumption: 92, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), ragefighterClass: 4);
        this.CreateSkill(1117, "Magic Pin Enhancement Skill", damage: 80, distance: 2, abilityConsumption: 15, manaConsumption: 23, dexterityRequirement: 200, strengthRequirement: 200, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), growLancerClass: 4);
        this.CreateSkill(1118, "Breche Enhancement Skill", damage: 230, distance: 6, abilityConsumption: 12, manaConsumption: 20, dexterityRequirement: 300, strengthRequirement: 300, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), growLancerClass: 4);
        this.CreateSkill(1119, "Shining Peak Enhancement Skill", DamageType.Physical, damage: 50, distance: 4, abilityConsumption: 12, manaConsumption: 15, strengthRequirement: 600, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 1), scalingStat2: (Stats.TotalStrength, 5), growLancerClass: 4);
        this.CreateSkill(1125, "Poison Storm", DamageType.Physical, damage: 25000, distance: 6, abilityConsumption: 30, manaConsumption: 40, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1126, "Frozen Slayer", DamageType.Physical, damage: 25000, distance: 6, abilityConsumption: 30, manaConsumption: 40, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1127, "Bloodying Hit", DamageType.Physical, damage: 25000, distance: 6, abilityConsumption: 30, manaConsumption: 40, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1129, "Poison Storm Damage Enhancement", DamageType.Physical, damage: 100, distance: 6, abilityConsumption: 30, manaConsumption: 40, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1130, "Frozen Slayer Damage Enhancement", DamageType.Physical, damage: 100, distance: 6, abilityConsumption: 30, manaConsumption: 40, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1131, "Bloodying Hit Damage Enhancement", DamageType.Physical, damage: 100, distance: 6, abilityConsumption: 30, manaConsumption: 40, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1133, "Poison Storm Blast Infection Effect", DamageType.Physical, damage: 100, distance: 6, abilityConsumption: 30, manaConsumption: 40, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1134, "Frozen Slayer Explosion Infection Effect", DamageType.Physical, damage: 100, distance: 6, abilityConsumption: 30, manaConsumption: 40, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1135, "Bloodying Hit Explosion Infection Effect", DamageType.Physical, damage: 100, distance: 6, abilityConsumption: 30, manaConsumption: 40, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1137, "Maximum Life Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1138, "4th Stat Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1139, "Base Defense Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1140, "4th Wing / Cloak Defense Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1141, "Increase DMG", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1142, "Increase Magic", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1143, "Increase Fourth Stats", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1144, "Increase Skill DMG", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1145, "Increase Fourth Wings DMG", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1146, "Increase DMG / Magic", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1147, "Spirit Hook Enhancement Skill", DamageType.Physical, damage: 255, distance: 3, abilityConsumption: 21, manaConsumption: 27, vitalityRequirement: 1480, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalVitality, 10), ragefighterClass: 4);
        this.CreateSkill(1148, "Magic Arrow Enhancement Skill", DamageType.Wizardry, damage: 10, distance: 8, abilityConsumption: 0, manaConsumption: 37, energyRequirement: 60, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 500), runeWizardClass: 4);
        this.CreateSkill(1149, "Plasma Ball Enhancement Skill", DamageType.Wizardry, damage: 40, distance: 8, abilityConsumption: 35, manaConsumption: 120, energyRequirement: 300, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), runeWizardClass: 4);
        this.CreateSkill(1150, "Lightning Storm Enhancement Skill", DamageType.Wizardry, damage: 300, distance: 8, abilityConsumption: 7, manaConsumption: 80, energyRequirement: 1080, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 100), runeWizardClass: 4);
        this.CreateSkill(1151, "Decrease Move Speed/Move Distance", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, runeWizardClass: 4);
        this.CreateSkill(1152, "Increase Plasma Attack Speed", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, runeWizardClass: 4);
        this.CreateSkill(1153, "Increase Splash Damage Area", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, runeWizardClass: 4);
        this.CreateSkill(1154, "Increase Splash Target", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, runeWizardClass: 4);
        this.CreateSkill(1155, "Deathside Enhancement", DamageType.Wizardry, damage: 95, distance: 6, abilityConsumption: 20, manaConsumption: 120, energyRequirement: 930, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), summonerClass: 4);
        this.CreateSkill(1156, "Weapon Minimum DMG Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1157, "Wizardry / Curse DMG Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1158, "Sword Inertia Enhancement", DamageType.Physical, damage: 10, distance: 7, abilityConsumption: 5, manaConsumption: 7, dexterityRequirement: 100, strengthRequirement: 50, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 150), slayerClass: 4);
        this.CreateSkill(1159, "Bat Flock Enhancement", DamageType.Physical, damage: 90, distance: 6, abilityConsumption: 7, manaConsumption: 15, dexterityRequirement: 380, strengthRequirement: 100, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 150), slayerClass: 4);
        this.CreateSkill(1160, "Pierce Attack Enhancement", DamageType.Physical, damage: 170, distance: 6, abilityConsumption: 10, manaConsumption: 20, dexterityRequirement: 1100, strengthRequirement: 300, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 150), slayerClass: 4);
        this.CreateSkill(1166, "Increase Poison debuff target count", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1167, "Increase Poison debuff target count", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1168, "Increase Hemorrhage debuff target count", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1169, "Increase Hemorrhage debuff target count", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1170, "Increase Freezing debuff target count", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1171, "Increase Freezing debuff target count", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1172, "Increase Poison debuff range", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1174, "Increase Hemorrhage debuff range", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1176, "Increase Freezing debuff range", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1178, "Increase Poison Debuff Success Rate", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1179, "Increase Poison Debuff Success Rate", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1180, "Increase Hemorrhage Debuff Success Rate", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1181, "Increase Hemorrhage Debuff Success Rate", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1182, "Increase Freezing Debuff Success Rate", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1183, "Increase Freezing Debuff Success Rate", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1184, "Increase Poison Debuff Success Rate", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1185, "Increase Poison Debuff Success Rate", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1186, "Increase Hemorrhage Debuff Success Rate", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1187, "Increase Hemorrhage Debuff Success Rate", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1188, "Increase Freezing Debuff Success Rate", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1189, "Increase Freezing Debuff Success Rate", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1190, "Increased Poison / Poison Debuff target count", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1191, "Increased Hemorrhage / Hemorrhage Debuff target count", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1192, "Increased Freezing / Freezing Debuff target count", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1193, "4th Wing / Cloak Enchantment", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1194, "4th Wing / Cloak Attack / Power Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1195, "4th Wing / Cloak Power / Curse Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1196, "Poison Storm Damage Enhancement", DamageType.Physical, damage: 100, distance: 6, abilityConsumption: 30, manaConsumption: 40, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1197, "Frozen Slayer Damage Enhancement", DamageType.Physical, damage: 100, distance: 6, abilityConsumption: 30, manaConsumption: 40, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1198, "Bloodying Hit Damage Enhancement", DamageType.Physical, damage: 100, distance: 6, abilityConsumption: 30, manaConsumption: 40, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1199, "Poison Storm Blast Infection Effect", DamageType.Physical, damage: 100, distance: 6, abilityConsumption: 30, manaConsumption: 40, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1200, "Frozen Slayer Explosion Infection Effect", DamageType.Physical, damage: 100, distance: 6, abilityConsumption: 30, manaConsumption: 40, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1201, "Bloodying Hit Explosion Infection Effect", DamageType.Physical, damage: 100, distance: 6, abilityConsumption: 30, manaConsumption: 40, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1202, "Strike of Destruction Enhancement Skill", DamageType.Physical, damage: 120, distance: 5, abilityConsumption: 24, manaConsumption: 30, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), knightClass: 4);
        this.CreateSkill(1203, "Sword Blow Enhancement Skill", DamageType.Physical, damage: 30, distance: 5, abilityConsumption: 17, manaConsumption: 19, strengthRequirement: 1090, elementalModifier: (ElementalType)6, knightClass: 4);
        this.CreateSkill(1204, "Solid Protection Enhancement Skill", damage: 120, distance: 0, abilityConsumption: 20, manaConsumption: 65, energyRequirement: 1052, knightClass: 4);
        this.CreateSkill(1205, "Protection - Maximum HP Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1206, "Protection - Party Member Attack Power Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1207, "Protection - HP Conversion Increase (%)", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1208, "Protection - Damage Conversion Increase (%)", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1209, "Protection - Party Member Defense Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1210, "Protection - Shield Defense Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1211, "Dark Plasma Enhancement Skill", DamageType.Wizardry, damage: 50, distance: 6, abilityConsumption: 15, manaConsumption: 19, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 4);
        this.CreateSkill(1212, "Ice Blast Enhancement Skill", DamageType.Wizardry, damage: 130, distance: 6, abilityConsumption: 10, manaConsumption: 14, energyRequirement: 1000, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 4);
        this.CreateSkill(1213, "Busting Flare Enhancement Skill", DamageType.Wizardry, damage: 50, distance: 6, abilityConsumption: 13, manaConsumption: 14, energyRequirement: 800, dexterityRequirement: 200, strengthRequirement: 100, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 4);
        this.CreateSkill(1214, "Chaos Blade Enhancement Skill", DamageType.Physical, damage: 350, distance: 3, abilityConsumption: 14, manaConsumption: 19, strengthRequirement: 900, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 750), magicGladiatorClass: 4);
        this.CreateSkill(1215, "Havok Spear Enhancement Skill", DamageType.Wizardry, damage: 150, distance: 6, abilityConsumption: 20, manaConsumption: 92, energyRequirement: 1073, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), magicGladiatorClass: 4);
        this.CreateSkill(1216, "Spear Storm Enhancement Skill", DamageType.Wizardry, damage: 160, distance: 6, abilityConsumption: 7, manaConsumption: 108, energyRequirement: 1160, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), whiteWizardClass: 4);
        this.CreateSkill(1217, "Marvel Burst Enhancement Skill", DamageType.Wizardry, damage: 125, distance: 6, abilityConsumption: 0, manaConsumption: 32, energyRequirement: 104, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), lemuriaClass: 4);
        this.CreateSkill(1218, "Unleash Marvel Enhancement Skill", DamageType.Wizardry, damage: 135, distance: 7, abilityConsumption: 0, manaConsumption: 113, energyRequirement: 700, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), lemuriaClass: 4);
        this.CreateSkill(1219, "Ultimate Force Enhancement Skill", DamageType.Wizardry, damage: 10, distance: 7, abilityConsumption: 6, manaConsumption: 95, energyRequirement: 1073, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), lemuriaClass: 4);
        this.CreateSkill(1220, "Shining Bird Enhancement Skill", DamageType.Wizardry, damage: 130, distance: 7, abilityConsumption: 0, manaConsumption: 32, energyRequirement: 70, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), whiteWizardClass: 4);
        this.CreateSkill(1221, "Dragon Violent Enhancement Skill", DamageType.Wizardry, damage: 140, distance: 7, abilityConsumption: 0, manaConsumption: 95, energyRequirement: 680, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), whiteWizardClass: 4);
        this.CreateSkill(1222, "Raining Arrow Enhancement Skill", DamageType.Physical, damage: 65, distance: 7, abilityConsumption: 8, manaConsumption: 19, dexterityRequirement: 1302, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 150), elfClass: 4);
        this.CreateSkill(1223, "Holy Bolt Enhancement Skill", DamageType.Wizardry, damage: 777, distance: 6, abilityConsumption: 0, manaConsumption: 30, energyRequirement: 1200, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), elfClass: 4);
        this.CreateSkill(1224, "Elemental Attack Power Enhancement Skill", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 50, energyRequirement: 1500, elfClass: 4);
        this.CreateSkill(1225, "Elemental Defense Enhancement Skill", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 50, energyRequirement: 1500, elfClass: 4);
        this.CreateSkill(1226, "Healing Enhancement Skill", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 40, energyRequirement: 52, elfClass: 4);
        this.CreateSkill(1227, "Party Healing Enhancement Skill", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 13, manaConsumption: 400, energyRequirement: 20, elfClass: 4);
        this.CreateSkill(1228, "Attack Power Enhancement Skill", DamageType.Wizardry, damage: 120, distance: 6, abilityConsumption: 0, manaConsumption: 55, energyRequirement: 92, elfClass: 4);
        this.CreateSkill(1229, "Defense Enhancement Skill", DamageType.Wizardry, damage: 120, distance: 6, abilityConsumption: 0, manaConsumption: 45, energyRequirement: 72, elfClass: 4);
        this.CreateSkill(1230, "Bless Enhancement Skill", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 22, manaConsumption: 130, energyRequirement: 20, elfClass: 4);
        this.CreateSkill(1231, "Charge Slash Enhancement Skill", DamageType.Physical, damage: 80, distance: 7, abilityConsumption: 0, manaConsumption: 3, dexterityRequirement: 80, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), illusionKnightClass: 4);
        this.CreateSkill(1232, "Wind Glaive Enhancement Skill", DamageType.Physical, damage: 150, distance: 6, abilityConsumption: 0, manaConsumption: 6, dexterityRequirement: 650, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 80), illusionKnightClass: 4);
        this.CreateSkill(1233, "Blade Storm Enhancement Skill", DamageType.Physical, damage: 300, distance: 6, abilityConsumption: 10, manaConsumption: 10, dexterityRequirement: 1150, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 50), illusionKnightClass: 4);
        this.CreateSkill(1234, "Illusion Avatar Enhancement Skill", damage: 200, distance: 6, abilityConsumption: 0, manaConsumption: 40, dexterityRequirement: 900, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), illusionKnightClass: 4);
        this.CreateSkill(1235, "Oversting Enhancement Skill", DamageType.Physical, damage: 320, distance: 6, abilityConsumption: 17, manaConsumption: 27, dexterityRequirement: 1470, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 80), growLancerClass: 4);
        this.CreateSkill(1236, "Wild Breath Enhancement Skill", DamageType.Physical, damage: 320, distance: 6, abilityConsumption: 19, manaConsumption: 35, strengthRequirement: 1020, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), growLancerClass: 4);
        this.CreateSkill(1237, "Alchemy: Angel Homunculus Enhancement Skill", DamageType.Wizardry, damage: 130, distance: 6, abilityConsumption: 0, manaConsumption: 35, energyRequirement: 70, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), alchemistClass: 4);
        this.CreateSkill(1238, "Alchemy: Ignition Bomber Enhancement Skill", DamageType.Wizardry, damage: 140, distance: 7, abilityConsumption: 0, manaConsumption: 95, energyRequirement: 680, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), alchemistClass: 4);
        this.CreateSkill(1239, "Alchemy: Countless Weapon Enhancement Skill", DamageType.Wizardry, damage: 160, distance: 6, abilityConsumption: 11, manaConsumption: 112, energyRequirement: 1025, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), alchemistClass: 4);
        this.CreateSkill(1240, "Spirit Blast Enhancement Skill", DamageType.Physical, damage: 350, distance: 6, abilityConsumption: 16, manaConsumption: 35, leadershipRequirement: 620, strengthRequirement: 800, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalLeadership, 50), scalingStat2: (Stats.TotalStrength, 100), darkLordClass: 4);
        this.CreateSkill(1241, "4th Stat Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1242, "4th Stat Increase", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 4, knightClass: 4, elfClass: 4, magicGladiatorClass: 4, darkLordClass: 4, summonerClass: 4, ragefighterClass: 4, growLancerClass: 4, runeWizardClass: 4, slayerClass: 4, gunCrusherClass: 4, whiteWizardClass: 4, lemuriaClass: 4, illusionKnightClass: 4, alchemistClass: 4, crusaderClass: 4);
        this.CreateSkill(1500, "Sword's Fury", damage: 0, distance: 0, abilityConsumption: 17, manaConsumption: 21, strengthRequirement: 1060, knightClass: 1);
        this.CreateSkill(1501, "Sword Blow", DamageType.Physical, damage: 30, distance: 5, abilityConsumption: 17, manaConsumption: 19, strengthRequirement: 1090, elementalModifier: (ElementalType)6, knightClass: 1);
        this.CreateSkill(1502, "Strong Belief", damage: 0, distance: 0, abilityConsumption: 25, manaConsumption: 80, energyRequirement: 1040, knightClass: 1);
        this.CreateSkill(1503, "Solid Protection", damage: 0, distance: 0, abilityConsumption: 20, manaConsumption: 65, energyRequirement: 1052, knightClass: 1);
        this.CreateSkill(1504, "Runic Spear Enhancement Skill", DamageType.Wizardry, damage: 20, distance: 7, abilityConsumption: 0, manaConsumption: 42, energyRequirement: 600, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 100), runeWizardClass: 4);
        this.CreateSkill(1505, "Rune Phrase Enhancement Skill", DamageType.Wizardry, damage: 160, distance: 7, abilityConsumption: 6, manaConsumption: 80, energyRequirement: 1180, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 100), runeWizardClass: 4);
        this.CreateSkill(1506, "Divine Fall Enhancement Skill", DamageType.Physical, damage: 150, distance: 6, abilityConsumption: 0, manaConsumption: 16, dexterityRequirement: 29, strengthRequirement: 60, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 45), crusaderClass: 4);
        this.CreateSkill(1507, "Holy Sweep Enhancement Skill", DamageType.Physical, damage: 300, distance: 5, abilityConsumption: 4, manaConsumption: 20, dexterityRequirement: 168, strengthRequirement: 420, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 10), crusaderClass: 4);
        this.CreateSkill(1508, "Sacred Impact Enhancement Skill", DamageType.Physical, damage: 350, distance: 7, abilityConsumption: 18, manaConsumption: 26, dexterityRequirement: 418, strengthRequirement: 1045, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 30), crusaderClass: 4);
        this.CreateSkill(2001, "Dark Plasma", DamageType.Wizardry, damage: 30, distance: 6, abilityConsumption: 10, manaConsumption: 22, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 2);
        this.CreateSkill(2002, "Ice Break", DamageType.Wizardry, damage: 180, distance: 6, abilityConsumption: 1, manaConsumption: 7, energyRequirement: 295, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 1);
        this.CreateSkill(2003, "Ice Blast", DamageType.Wizardry, damage: 140, distance: 6, abilityConsumption: 10, manaConsumption: 13, energyRequirement: 1000, elementalModifier: (ElementalType)6, gunCrusherClass: 3);
        this.CreateSkill(2004, "Death Fire", DamageType.Wizardry, damage: 160, distance: 6, abilityConsumption: 1, manaConsumption: 5, energyRequirement: 100, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 1);
        this.CreateSkill(2005, "Bursting Flare", DamageType.Wizardry, damage: 70, distance: 6, abilityConsumption: 13, manaConsumption: 10, energyRequirement: 800, dexterityRequirement: 200, strengthRequirement: 100, elementalModifier: (ElementalType)6, gunCrusherClass: 3);
        this.CreateSkill(2006, "Death Ice", DamageType.Wizardry, damage: 160, distance: 6, abilityConsumption: 1, manaConsumption: 5, energyRequirement: 100, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 1);
        this.CreateSkill(2007, "Beginner Bless", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 20, energyRequirement: 52, lemuriaClass: 1);
        this.CreateSkill(2008, "Beginner Recovery", damage: 0, distance: 6, abilityConsumption: 10, manaConsumption: 40, energyRequirement: 168, lemuriaClass: 2);
        this.CreateSkill(2009, "Beginner Basic Defense Improvement", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 30, energyRequirement: 72, lemuriaClass: 1);
        this.CreateSkill(2010, "Beginner Attack Power Improvement", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 40, energyRequirement: 92, lemuriaClass: 1);
        this.CreateSkill(2011, "Beginner Bless", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 10, manaConsumption: 40, energyRequirement: 150, lemuriaClass: 1);
        this.CreateSkill(2012, "Chaos Blade", DamageType.Physical, damage: 350, distance: 3, abilityConsumption: 14, manaConsumption: 19, strengthRequirement: 900, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 700), magicGladiatorClass: 3);
        this.CreateSkill(2013, "Havok Spear", DamageType.Wizardry, damage: 150, distance: 6, abilityConsumption: 20, manaConsumption: 92, energyRequirement: 1073, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), magicGladiatorClass: 3);
        this.CreateSkill(2014, "Spiral Charge", damage: 0, distance: 0, abilityConsumption: 17, manaConsumption: 21, strengthRequirement: 900, magicGladiatorClass: 3);
        this.CreateSkill(2015, "Crusher Charge", damage: 0, distance: 0, abilityConsumption: 17, manaConsumption: 21, strengthRequirement: 900, magicGladiatorClass: 3);
        this.CreateSkill(2016, "Elemental Charge", damage: 0, distance: 0, abilityConsumption: 20, manaConsumption: 65, energyRequirement: 1073, magicGladiatorClass: 3);
        this.CreateSkill(2017, "Chaos Blade Magic Explosion", damage: 400, distance: 6, abilityConsumption: 0, manaConsumption: 0, scalingStat1: (Stats.TotalStrength, 700), magicGladiatorClass: 3);
        this.CreateSkill(2018, "Fire Blood Magic Explosion", damage: 200, distance: 6, abilityConsumption: 0, manaConsumption: 0, scalingStat1: (Stats.TotalStrength, 700), magicGladiatorClass: 3);
        this.CreateSkill(2019, "Ice Blood Magic Explosion", damage: 200, distance: 6, abilityConsumption: 0, manaConsumption: 0, scalingStat1: (Stats.TotalStrength, 700), magicGladiatorClass: 3);
        this.CreateSkill(2020, "Havok Spear Nova", damage: 200, distance: 6, abilityConsumption: 0, manaConsumption: 0, scalingStat1: (Stats.TotalEnergy, 150), magicGladiatorClass: 3);
        this.CreateSkill(2021, "Fixed Fire", damage: 0, distance: 0, abilityConsumption: 50, manaConsumption: 100, energyRequirement: 300, gunCrusherClass: 1);
        this.CreateSkill(2022, "Bond", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0);
        this.CreateSkill(2023, "Raining Arrow", DamageType.Physical, damage: 65, distance: 7, abilityConsumption: 7, manaConsumption: 15, dexterityRequirement: 1302, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 150), elfClass: 3);
        this.CreateSkill(2024, "Dex Booster", damage: 0, distance: 0, abilityConsumption: 12, manaConsumption: 10, dexterityRequirement: 500, elfClass: 3);
        this.CreateSkill(2025, "Holy Bolt", DamageType.Wizardry, damage: 777, distance: 6, abilityConsumption: 0, manaConsumption: 20, energyRequirement: 1200, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), elfClass: 3);
        this.CreateSkill(2026, "Improve Elemental Attack Power", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 40, energyRequirement: 1500, elfClass: 3);
        this.CreateSkill(2027, "Improve Elemental Defense", DamageType.Wizardry, damage: 0, distance: 6, abilityConsumption: 0, manaConsumption: 40, energyRequirement: 1500, elfClass: 3);
        this.CreateSkill(2028, "Charge Slash", DamageType.Physical, damage: 80, distance: 6, abilityConsumption: 0, manaConsumption: 3, dexterityRequirement: 80, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), illusionKnightClass: 1);
        this.CreateSkill(2029, "Wind Glaive", DamageType.Physical, damage: 150, distance: 5, abilityConsumption: 0, manaConsumption: 6, dexterityRequirement: 650, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 80), illusionKnightClass: 1);
        this.CreateSkill(2030, "Blade Storm", DamageType.Physical, damage: 220, distance: 6, abilityConsumption: 10, manaConsumption: 10, dexterityRequirement: 1150, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 50), illusionKnightClass: 3);
        this.CreateSkill(2031, "Illusion Avatar", DamageType.Physical, damage: 200, distance: 6, abilityConsumption: 0, manaConsumption: 40, dexterityRequirement: 900, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 80), illusionKnightClass: 2);
        this.CreateSkill(2032, "Illusion Blade", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 40, illusionKnightClass: 1);
        this.CreateSkill(2036, "Oversting", DamageType.Physical, damage: 320, distance: 6, abilityConsumption: 12, manaConsumption: 25, dexterityRequirement: 1470, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 80), growLancerClass: 3);
        this.CreateSkill(2037, "Meteor Storm of Gale", DamageType.Wizardry, damage: 105, distance: 6, abilityConsumption: 20, manaConsumption: 105, energyRequirement: 1160, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), wizardClass: 5);
        this.CreateSkill(2040, "Sword Blow of Saturation", DamageType.Physical, damage: 30, distance: 5, abilityConsumption: 17, manaConsumption: 19, strengthRequirement: 1090, elementalModifier: (ElementalType)6, knightClass: 5);
        this.CreateSkill(2043, "Destruction of Gale", DamageType.Physical, damage: 120, distance: 5, abilityConsumption: 24, manaConsumption: 30, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), knightClass: 5);
        this.CreateSkill(2044, "Raining Arrow of Saturation", DamageType.Physical, damage: 65, distance: 7, abilityConsumption: 8, manaConsumption: 19, dexterityRequirement: 1302, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 150), elfClass: 5);
        this.CreateSkill(2047, "Holy Bolt of Gale", DamageType.Wizardry, damage: 777, distance: 6, abilityConsumption: 0, manaConsumption: 30, energyRequirement: 1200, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 150), elfClass: 5);
        this.CreateSkill(2049, "Chaos Blade of Saturation", DamageType.Physical, damage: 350, distance: 3, abilityConsumption: 14, manaConsumption: 19, strengthRequirement: 900, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 750), magicGladiatorClass: 5);
        this.CreateSkill(2051, "Havok Spear of Wrath", DamageType.Wizardry, damage: 150, distance: 6, abilityConsumption: 20, manaConsumption: 92, energyRequirement: 1073, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), magicGladiatorClass: 5);
        this.CreateSkill(2054, "Wind Soul of Saturation", DamageType.Physical, damage: 130, distance: 6, abilityConsumption: 19, manaConsumption: 38, strengthRequirement: 717, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), darkLordClass: 5);
        this.CreateSkill(2058, "Fire Beast of Saturation", DamageType.Wizardry, damage: 100, distance: 7, abilityConsumption: 11, manaConsumption: 95, energyRequirement: 1220, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 170), summonerClass: 5);
        this.CreateSkill(2060, "Death Scythe of Fury", DamageType.Wizardry, damage: 95, distance: 6, abilityConsumption: 20, manaConsumption: 120, energyRequirement: 930, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 200), summonerClass: 5);
        this.CreateSkill(2061, "Dark Side of Saturation", DamageType.Physical, damage: 0, distance: 6, abilityConsumption: 7, manaConsumption: 92, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 100), ragefighterClass: 5);
        this.CreateSkill(2063, "Spirit Hook of Saturation", DamageType.Physical, damage: 255, distance: 3, abilityConsumption: 21, manaConsumption: 27, vitalityRequirement: 1480, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalVitality, 10), ragefighterClass: 5);
        this.CreateSkill(2065, "Oversting of Saturation", DamageType.Physical, damage: 320, distance: 6, abilityConsumption: 17, manaConsumption: 27, dexterityRequirement: 1470, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 80), growLancerClass: 5);
        this.CreateSkill(2068, "Lightning Storm of Gale", DamageType.Wizardry, damage: 300, distance: 8, abilityConsumption: 7, manaConsumption: 80, energyRequirement: 1080, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 100), runeWizardClass: 5);
        this.CreateSkill(2071, "Pierce Attack of Saturation", DamageType.Physical, damage: 170, distance: 6, abilityConsumption: 10, manaConsumption: 20, dexterityRequirement: 1100, strengthRequirement: 300, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 150), slayerClass: 5);
        this.CreateSkill(2073, "Bursting Flare of Gale", DamageType.Wizardry, damage: 50, distance: 6, abilityConsumption: 13, manaConsumption: 14, energyRequirement: 800, dexterityRequirement: 200, strengthRequirement: 100, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 80), gunCrusherClass: 5);
        this.CreateSkill(2076, "Ultimate Storm of Saturation", DamageType.Wizardry, damage: 10, distance: 7, abilityConsumption: 6, manaConsumption: 95, energyRequirement: 1073, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), lemuriaClass: 5);
        this.CreateSkill(2080, "Spear Storm of Saturation", DamageType.Wizardry, damage: 160, distance: 6, abilityConsumption: 7, manaConsumption: 108, energyRequirement: 1160, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), whiteWizardClass: 5);
        this.CreateSkill(2082, "Blade Storm of Saturation", DamageType.Physical, damage: 300, distance: 6, abilityConsumption: 10, manaConsumption: 10, dexterityRequirement: 1150, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalAgility, 50), illusionKnightClass: 5);
        this.CreateSkill(2085, "Wild Breath", DamageType.Physical, damage: 320, distance: 6, abilityConsumption: 18, manaConsumption: 35, strengthRequirement: 1020, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), growLancerClass: 3);
        this.CreateSkill(2086, "Nuke (activation)", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0);
        this.CreateSkill(2087, "Bolt (activation)", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0);
        this.CreateSkill(2088, "Wide Area (activation)", damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 0);
        this.CreateSkill(2089, "Wild Breath of Gale", DamageType.Physical, damage: 320, distance: 6, abilityConsumption: 19, manaConsumption: 35, strengthRequirement: 1020, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), growLancerClass: 5);
        this.CreateSkill(2090, "Alchemy: Confusion Stone", damage: 0, distance: 6, abilityConsumption: 25, manaConsumption: 70, energyRequirement: 850, alchemistClass: 1);
        this.CreateSkill(2091, "Alchemy: Angel Homunculus", DamageType.Wizardry, damage: 130, distance: 6, abilityConsumption: 0, manaConsumption: 5, energyRequirement: 70, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 45), alchemistClass: 1);
        this.CreateSkill(2092, "Alchemy: Ignition Bomber", DamageType.Wizardry, damage: 140, distance: 6, abilityConsumption: 0, manaConsumption: 30, energyRequirement: 680, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), alchemistClass: 1);
        this.CreateSkill(2093, "Alchemy: Countless Weapon", DamageType.Wizardry, damage: 160, distance: 6, abilityConsumption: 10, manaConsumption: 108, energyRequirement: 1025, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), alchemistClass: 3);
        this.CreateSkill(2094, "Fiery Countless Weapon", DamageType.Wizardry, damage: 160, distance: 6, abilityConsumption: 13, manaConsumption: 120, energyRequirement: 1025, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 60), alchemistClass: 5);
        this.CreateSkill(2096, "Spirit Blast of Anger", DamageType.Physical, damage: 350, distance: 6, abilityConsumption: 16, manaConsumption: 35, leadershipRequirement: 620, strengthRequirement: 800, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalLeadership, 50), scalingStat2: (Stats.TotalStrength, 100), darkLordClass: 5);
        this.CreateSkill(2097, "Crown Force", DamageType.Wizardry, damage: 0, distance: 0, abilityConsumption: 20, manaConsumption: 80, energyRequirement: 300, leadershipRequirement: 400, darkLordClass: 1);
        this.CreateSkill(2098, "Divine Force", DamageType.Wizardry, damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 100, leadershipRequirement: 500, darkLordClass: 3);
        this.CreateSkill(2099, "Divine Aura", DamageType.Wizardry, damage: 0, distance: 0, abilityConsumption: 0, manaConsumption: 100, leadershipRequirement: 500, darkLordClass: 3);
        this.CreateSkill(2100, "Battle Glory", damage: 0, distance: 0, abilityConsumption: 30, manaConsumption: 40, leadershipRequirement: 200, strengthRequirement: 200, darkLordClass: 1);
        this.CreateSkill(2101, "Runic Spear", DamageType.Wizardry, damage: 20, distance: 7, abilityConsumption: 0, manaConsumption: 15, energyRequirement: 600, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 100), runeWizardClass: 2);
        this.CreateSkill(2102, "Rune Phrase", DamageType.Wizardry, damage: 160, distance: 7, abilityConsumption: 5, manaConsumption: 70, energyRequirement: 1180, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 100), runeWizardClass: 3);
        this.CreateSkill(2103, "Rune Phrase of Saturation", DamageType.Wizardry, damage: 160, distance: 7, abilityConsumption: 6, manaConsumption: 90, energyRequirement: 1180, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalEnergy, 100), runeWizardClass: 5);
        this.CreateSkill(2104, "Special Praise", damage: 160, distance: 7, abilityConsumption: 0, manaConsumption: 0, scalingStat1: (Stats.TotalEnergy, 100), runeWizardClass: 3);
        this.CreateSkill(2105, "Divine Fall", DamageType.Physical, damage: 150, distance: 6, abilityConsumption: 0, manaConsumption: 5, dexterityRequirement: 29, strengthRequirement: 60, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 45), crusaderClass: 1);
        this.CreateSkill(2106, "Holy Sweep", DamageType.Physical, damage: 300, distance: 5, abilityConsumption: 1, manaConsumption: 13, dexterityRequirement: 168, strengthRequirement: 420, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 10), crusaderClass: 2);
        this.CreateSkill(2107, "Sacred Impact", DamageType.Physical, damage: 350, distance: 7, abilityConsumption: 15, manaConsumption: 20, dexterityRequirement: 418, strengthRequirement: 1045, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 50), crusaderClass: 3);
        this.CreateSkill(2108, "Lugard's Guardian: Punishment", damage: 0, distance: 8, abilityConsumption: 30, manaConsumption: 200, energyRequirement: 200, crusaderClass: 2);

        this.InitializeEffects();
        this.MapSkillsToEffects();
        this.InitializeMasterSkillData();
        this.CreateSpecialSummonMonsters();
        this.CreateSkillCombos();
    }

    // ReSharper disable once UnusedMember.Local
    private void InitializeNextSeasonMasterSkills()
    {
        // Common:
        


        /*
        this.CreateSkill(SkillNumber.CastInvincibility, "Cast Invincibility", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.ArmorSetBonusInc, "Armor Set Bonus Inc", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.Vengeance, "Vengeance", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.EnergyIncrease, "Energy Increase", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.StaminaIncrease, "Stamina Increase", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.AgilityIncrease, "Agility Increase", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.StrengthIncrease, "Strength Increase", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.SwellLifeMastery, "Swell Life Mastery", [CharacterClassNumber.BladeMaster], damage: 7, abilityConsumption: 30, manaConsumption: 28, levelRequirement: 120);
        this.CreateSkill(SkillNumber.MaximumAttackPowerInc, "Maximum Attack Power Inc", [CharacterClassNumber.BladeMaster, CharacterClassNumber.DuelMaster, CharacterClassNumber.LordEmperor], DamageType.Physical, 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.Inccritdamagerate, "Inc crit damage rate", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 7, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.RestoresallMana, "Restores all Mana", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.RestoresallHp, "Restores all HP", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.Incexcdamagerate, "Inc exc damage rate", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.Incdoubledamagerate, "Inc double damage rate", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncchanceofignoreDef, "Inc chance of ignore Def", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.RestoresallSd, "Restores all SD", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.Inctripledamagerate, "Inc triple damage rate", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 1, skillType: SkillType.PassiveBoost);

        // Blade Master:
        this.CreateSkill(SkillNumber.WingofStormAbsPowUp, "Wing of Storm Abs PowUp", [CharacterClassNumber.BladeMaster], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.WingofStormDefPowUp, "Wing of Storm Def PowUp", [CharacterClassNumber.BladeMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IronDefense, "Iron Defense", ClassConsts.ALL_MASTERS, damage: 1);
        this.CreateSkill(SkillNumber.WingofStormAttPowUp, "Wing of Storm Att PowUp", [CharacterClassNumber.BladeMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DeathStabProficiency, "Death Stab Proficiency", [CharacterClassNumber.BladeMaster], DamageType.Physical, 7, 2, 26, 30, 160, elementalModifier: ElementalType.Wind);
        this.CreateSkill(SkillNumber.StrikeofDestrProf, "Strike of Destr Prof", [CharacterClassNumber.BladeMaster], DamageType.Physical, 7, 5, 24, 30, 100, elementalModifier: ElementalType.Ice);
        this.CreateSkill(SkillNumber.MaximumAgIncrease, "Maximum AG Increase", ClassConsts.ALL_MASTERS_EXCEPT_RF, damage: 8, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DeathStabMastery, "Death Stab Mastery", [CharacterClassNumber.BladeMaster], DamageType.Physical, 7, 2, 26, 30, 160, elementalModifier: ElementalType.Wind);
        this.CreateSkill(SkillNumber.StrikeofDestrMast, "Strike of Destr Mast", [CharacterClassNumber.BladeMaster], DamageType.Physical, 1, 5, 24, 30, 100, elementalModifier: ElementalType.Ice);
        this.CreateSkill(SkillNumber.BloodStorm, "Blood Storm", [CharacterClassNumber.BladeMaster, CharacterClassNumber.DuelMaster], DamageType.Physical, 25, 3, 29, 87);
        this.CreateSkill(SkillNumber.ComboStrengthener, "Combo Strengthener", [CharacterClassNumber.BladeMaster], DamageType.Physical, 7, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.BloodStormStrengthener, "Blood Storm Strengthener", [CharacterClassNumber.BladeMaster, CharacterClassNumber.DuelMaster], DamageType.Physical, 22, 3, 29, 87);

        // Grand Master:
        this.CreateSkill(SkillNumber.EternalWingsAbsPowUp, "Eternal Wings Abs PowUp", [CharacterClassNumber.GrandMaster], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.EternalWingsDefPowUp, "Eternal Wings Def PowUp", [CharacterClassNumber.GrandMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.EternalWingsAttPowUp, "Eternal Wings Att PowUp", [CharacterClassNumber.GrandMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.MeteorStrengthener, "Meteor Strengthener", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 4, 6, manaConsumption: 13, levelRequirement: 21, energyRequirement: 100, elementalModifier: ElementalType.Earth);
        this.CreateSkill(SkillNumber.IceStormStrengthener, "Ice Storm Strengthener", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 22, 6, 5, 110, 93, 223, elementalModifier: ElementalType.Ice);
        this.CreateSkill(SkillNumber.NovaStrengthener, "Nova Strengthener", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 22, 6, 49, 198, 100, 258, elementalModifier: ElementalType.Fire);
        this.CreateSkill(SkillNumber.IceStormMastery, "Ice Storm Mastery", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 1, 6, 5, 110, 93, 223, elementalModifier: ElementalType.Ice);
        this.CreateSkill(SkillNumber.MeteorMastery, "Meteor Mastery", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 1, 6, manaConsumption: 14, levelRequirement: 21, energyRequirement: 100, elementalModifier: ElementalType.Earth);
        this.CreateSkill(SkillNumber.NovaCastStrengthener, "Nova Cast Strengthener", [CharacterClassNumber.GrandMaster], DamageType.Wizardry, 22, 6, 49, 198, 100, 258, elementalModifier: ElementalType.Fire);
        this.CreateSkill(SkillNumber.SoulBarrierMastery, "Soul Barrier Mastery", [CharacterClassNumber.GrandMaster], damage: 7, distance: 6, abilityConsumption: 28, manaConsumption: 92, levelRequirement: 77, energyRequirement: 126);
        this.CreateSkill(SkillNumber.MaximumWizardryInc, "Maximum Wizardry Inc", [CharacterClassNumber.GrandMaster, CharacterClassNumber.DuelMaster], damage: 3, skillType: SkillType.PassiveBoost);

        // High Elf:
        this.CreateSkill(SkillNumber.IllusionWingsAbsPowUp, "Illusion Wings Abs PowUp", [CharacterClassNumber.HighElf], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IllusionWingsDefPowUp, "Illusion Wings Def PowUp", [CharacterClassNumber.HighElf], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.MultiShotStreng, "Multi-Shot Streng", [CharacterClassNumber.HighElf], DamageType.Physical, 22, 6, 7, 11, 100, skillType: SkillType.AreaSkillAutomaticHits);
        this.CreateSkill(SkillNumber.IllusionWingsAttPowUp, "Illusion Wings Att PowUp", [CharacterClassNumber.HighElf], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.Cure, "Cure", [CharacterClassNumber.HighElf], distance: 6, abilityConsumption: 10, manaConsumption: 72);
        this.CreateSkill(SkillNumber.PartyHealing, "Party Healing", [CharacterClassNumber.HighElf], distance: 6, abilityConsumption: 12, manaConsumption: 66, energyRequirement: 100);
        this.CreateSkill(SkillNumber.PoisonArrow, "Poison Arrow", [CharacterClassNumber.HighElf], DamageType.Physical, 27, 6, 27, 22, elementalModifier: ElementalType.Poison);
        this.CreateSkill(SkillNumber.SummonedMonsterStr3, "Summoned Monster Str (3)", [CharacterClassNumber.HighElf], damage: 16, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.PartyHealingStr, "Party Healing Str", [CharacterClassNumber.HighElf], damage: 22, distance: 6, abilityConsumption: 13, manaConsumption: 72, energyRequirement: 100);
        this.CreateSkill(SkillNumber.Bless, "Bless", [CharacterClassNumber.HighElf], distance: 6, abilityConsumption: 18, manaConsumption: 108, energyRequirement: 100);
        this.CreateSkill(SkillNumber.MultiShotMastery, "Multi-Shot Mastery", [CharacterClassNumber.HighElf], DamageType.Physical, 1, 6, 8, 12, 100, skillType: SkillType.AreaSkillAutomaticHits);
        this.CreateSkill(SkillNumber.SummonSatyros, "Summon Satyros", [CharacterClassNumber.HighElf], abilityConsumption: 52, manaConsumption: 525, energyRequirement: 280);
        this.CreateSkill(SkillNumber.BlessStrengthener, "Bless Strengthener", [CharacterClassNumber.HighElf], damage: 10, distance: 6, abilityConsumption: 20, manaConsumption: 118, energyRequirement: 100);
        this.CreateSkill(SkillNumber.PoisonArrowStr, "Poison Arrow Str", [CharacterClassNumber.HighElf], DamageType.Physical, 22, 6, 29, 24, elementalModifier: ElementalType.Poison);
        this.CreateSkill(SkillNumber.MaximumAttPowerInc, "Maximum Att Power Inc", [CharacterClassNumber.HighElf], DamageType.Physical, 3, skillType: SkillType.PassiveBoost);

        // Dimension Master (Summoner):
        this.CreateSkill(SkillNumber.DimensionWingsAbsPowUp, "DimensionWings Abs PowUp", [CharacterClassNumber.DimensionMaster], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DimensionWingsDefPowUp, "DimensionWings Def PowUp", [CharacterClassNumber.DimensionMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DimensionWingsAttPowUp, "DimensionWings Att PowUp", [CharacterClassNumber.DimensionMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.WeaknessStrengthener, "Weakness Strengthener", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 3, 6, 17, 55, 93, 173);
        this.CreateSkill(SkillNumber.InnovationStrengthener, "Innovation Strengthener", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 3, 6, 17, 77, 111, 201);
        this.CreateSkill(SkillNumber.Blind, "Blind", [CharacterClassNumber.DimensionMaster], DamageType.Curse, distance: 3, abilityConsumption: 25, manaConsumption: 115, energyRequirement: 201);
        this.CreateSkill(SkillNumber.DrainLifeMastery, "Drain Life Mastery", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 17, 6, manaConsumption: 62, levelRequirement: 35, energyRequirement: 93);
        this.CreateSkill(SkillNumber.BlindStrengthener, "Blind Strengthener", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 1, 3, 27, 126, energyRequirement: 201);
        this.CreateSkill(SkillNumber.BerserkerMastery, "Berserker Mastery", [CharacterClassNumber.DimensionMaster], DamageType.Curse, 10, 5, 90, 181, 83, 181);
        this.CreateSkill(SkillNumber.MaximumWizCurseInc, "Maximum Wiz/Curse Inc", [CharacterClassNumber.DimensionMaster], damage: 3, skillType: SkillType.PassiveBoost);

        // Duel Master (MG):
        this.CreateSkill(SkillNumber.WingofRuinAbsPowUp, "Wing of Ruin Abs PowUp", [CharacterClassNumber.DuelMaster], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.WingofRuinDefPowUp, "Wing of Ruin Def PowUp", [CharacterClassNumber.DuelMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.WingofRuinAttPowUp, "Wing of Ruin Att PowUp", [CharacterClassNumber.DuelMaster], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IceMasteryDuelMaster, "Ice Mastery", [CharacterClassNumber.DuelMaster], DamageType.Physical, 1, 6, manaConsumption: 46, levelRequirement: 25, energyRequirement: 100, elementalModifier: ElementalType.Ice);
        this.CreateSkill(SkillNumber.FlameStrikeStrengthen, "Flame Strike Strengthen", [CharacterClassNumber.DuelMaster], DamageType.Physical, 22, 3, 37, 30, elementalModifier: ElementalType.Fire);
        this.CreateSkill(SkillNumber.FireSlashMastery, "Fire Slash Mastery", [CharacterClassNumber.DuelMaster], damage: 7, distance: 3, abilityConsumption: 24, manaConsumption: 17, elementalModifier: ElementalType.Poison);
        this.CreateSkill(SkillNumber.FlameStrikeMastery, "Flame Strike Mastery", [CharacterClassNumber.DuelMaster], DamageType.Physical, 7, 3, 40, 33, elementalModifier: ElementalType.Fire);
        this.CreateSkill(SkillNumber.EarthPrison, "Earth Prison", [CharacterClassNumber.GrandMaster, CharacterClassNumber.DuelMaster], DamageType.Physical, 26, 3, 15, 180, energyRequirement: 127, elementalModifier: ElementalType.Earth);
        this.CreateSkill(SkillNumber.GiganticStormStr, "Gigantic Storm Str", [CharacterClassNumber.DuelMaster], DamageType.Physical, 22, 6, 11, 132, 220, 118, elementalModifier: ElementalType.Wind);
        this.CreateSkill(SkillNumber.EarthPrisonStr, "Earth Prison Str", [CharacterClassNumber.GrandMaster, CharacterClassNumber.DuelMaster], DamageType.Physical, 22, 3, 17, 198, energyRequirement: 127, elementalModifier: ElementalType.Earth);

        // Lord Emperor (DL):
        this.CreateSkill(SkillNumber.EmperorCapeAbsPowUp, "Emperor Cape Abs PowUp", [CharacterClassNumber.LordEmperor], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.EmperorCapeDefPowUp, "Emperor Cape Def PowUp", [CharacterClassNumber.LordEmperor], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.AddsCommandStat, "Adds Command Stat", [CharacterClassNumber.LordEmperor], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.EmperorCapeAttPowUp, "Emperor Cape Att PowUp", [CharacterClassNumber.LordEmperor], damage: 17, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.ElectricSparkStreng, "Electric Spark Streng", [CharacterClassNumber.LordEmperor], DamageType.Physical, 3, 10, 150, levelRequirement: 92, energyRequirement: 29, leadershipRequirement: 340, skillType: SkillType.AreaSkillAutomaticHits);
        this.CreateSkill(SkillNumber.FireScreamMastery, "Fire Scream Mastery", [CharacterClassNumber.LordEmperor], DamageType.Physical, 5, 6, 12, 49, 102, 32, 70);
        this.CreateSkill(SkillNumber.IronDefenseLordEmperor, "Iron Defense", [CharacterClassNumber.LordEmperor], damage: 28, abilityConsumption: 29, manaConsumption: 64);
        this.CreateSkill(SkillNumber.CriticalDamageIncM, "Critical Damage Inc M", [CharacterClassNumber.LordEmperor], damage: 1, abilityConsumption: 110, manaConsumption: 110, levelRequirement: 82, energyRequirement: 25, leadershipRequirement: 300);
        this.CreateSkill(SkillNumber.ChaoticDiseierStr, "Chaotic Diseier Str", [CharacterClassNumber.LordEmperor], DamageType.Physical, 22, 6, 22, 75, 100, 16, skillType: SkillType.AreaSkillAutomaticHits);
        this.CreateSkill(SkillNumber.IronDefenseStr, "Iron Defense Str", [CharacterClassNumber.LordEmperor], damage: 3, abilityConsumption: 31, manaConsumption: 70);
        this.CreateSkill(SkillNumber.DarkSpiritStr4, "Dark Spirit Str (4)", [CharacterClassNumber.LordEmperor], damage: 23, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.DarkSpiritStr5, "Dark Spirit Str (5)", [CharacterClassNumber.LordEmperor], damage: 1, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.SpiritLord, "Spirit Lord", [CharacterClassNumber.LordEmperor], damage: 1, skillType: SkillType.PassiveBoost);

        // Fist Master (Rage Fighter):
        this.CreateSkill(SkillNumber.CastInvincibilityFistMaster, "Cast Invincibility", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseMaximumAg, "Increase Maximum AG", [CharacterClassNumber.FistMaster], damage: 37, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseMaximumAttackPower, "Increase Maximum Attack Power", [CharacterClassNumber.FistMaster], damage: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreasesCritDamageChance, "Increases Crit Damage Chance", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.RecoverManaFully, "Recover Mana Fully", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.RecoversHpFully, "Recovers HP Fully", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseExcDamageChance, "Increase Exc Damage Chance", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseDoubleDamageChance, "Increase Double Damage Chance", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseIgnoreDefChance, "Increase Ignore Def Chance", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.RecoversSdFully, "Recovers SD Fully", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        this.CreateSkill(SkillNumber.IncreaseTripleDamageChance, "Increase Triple Damage Chance", [CharacterClassNumber.FistMaster], damage: 38, skillType: SkillType.PassiveBoost);
        */
    }

    private void CreateSkillCombos()
    {
        var bladeKnightCombo = this.Context.CreateNew<SkillComboDefinition>();
        var bladeKnight = this.GameConfiguration.CharacterClasses.First(c => c.Number == (byte)CharacterClassNumber.BladeKnight);
        bladeKnight.ComboDefinition = bladeKnightCombo;

        bladeKnightCombo.Name = "Blade Knight Combo";

        this.AddComboStep(SkillNumber.Slash, 1, bladeKnightCombo);
        this.AddComboStep(SkillNumber.Cyclone, 1, bladeKnightCombo);
        this.AddComboStep(SkillNumber.Lunge, 1, bladeKnightCombo);
        this.AddComboStep(SkillNumber.FallingSlash, 1, bladeKnightCombo);
        this.AddComboStep(SkillNumber.Uppercut, 1, bladeKnightCombo);

        this.AddComboStep(SkillNumber.TwistingSlash, 2, bladeKnightCombo);
        this.AddComboStep(SkillNumber.AngerBlow, 2, bladeKnightCombo);
        this.AddComboStep(SkillNumber.DeathStab, 2, bladeKnightCombo);
        this.AddComboStep(SkillNumber.StrikeofDestruction, 2, bladeKnightCombo);

        this.AddComboStep(SkillNumber.TwistingSlash, 3, bladeKnightCombo, true);
        this.AddComboStep(SkillNumber.AngerBlow, 3, bladeKnightCombo, true);
        this.AddComboStep(SkillNumber.DeathStab, 3, bladeKnightCombo, true);
    }

    private void AddComboStep(SkillNumber skillNumber, int order, SkillComboDefinition comboDefinition, bool isFinal = false)
    {
        var skill = this.GameConfiguration.Skills.First(s => s.Number == (short)skillNumber);
        var step = this.Context.CreateNew<SkillComboStep>();
        comboDefinition.Steps.Add(step);
        comboDefinition.MaximumCompletionTime = TimeSpan.FromSeconds(3);
        step.Skill = skill;
        step.Order = order;
        step.IsFinalStep = isFinal;
    }




    private void InitializeEffects()
    {
        new SoulBarrierEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new LifeSwellEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new CriticalDamageIncreaseEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new DefenseEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new GreaterDamageEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new GreaterDefenseEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new HealEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new ShieldRecoverEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new InfiniteArrowEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new DefenseReductionEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new InvisibleEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new IgnoreDefenseEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new IncreaseHealthEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new IncreaseBlockEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new WizardryEnhanceEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new AlcoholEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new SoulPotionEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new BlessPotionEffectInitializer(this.Context, this.GameConfiguration).Initialize();
        new BerserkerEffectInitializer(this.Context, this.GameConfiguration).Initialize();
    }

    private void MapSkillsToEffects()
    {
        foreach (var effectOfSkill in EffectsOfSkills)
        {
            var skill = this.GameConfiguration.Skills.First(s => s.Number == (short)effectOfSkill.Key);
            var effect = this.GameConfiguration.MagicEffects.First(e => e.Number == (short)effectOfSkill.Value);
            skill.MagicEffectDef = effect;
        }
    }

    /// <summary>
    /// Initializes the master skill data.
    /// </summary>
    private void InitializeMasterSkillData()
    {
        // Roots:
        var leftRoot = this.Context.CreateNew<MasterSkillRoot>();
        leftRoot.Name = "Left (Common Skills)";
        this._masterSkillRoots.Add(1, leftRoot);
        this.GameConfiguration.MasterSkillRoots.Add(leftRoot);
        var middleRoot = this.Context.CreateNew<MasterSkillRoot>();
        middleRoot.Name = "Middle Root";
        this._masterSkillRoots.Add(2, middleRoot);
        this.GameConfiguration.MasterSkillRoots.Add(middleRoot);
        var rightRoot = this.Context.CreateNew<MasterSkillRoot>();
        rightRoot.Name = "Right Root";
        this._masterSkillRoots.Add(3, rightRoot);
        this.GameConfiguration.MasterSkillRoots.Add(rightRoot);


        this.CreateSkill(300, "Durability Reduction (1)", damage: 37, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(301, "PvP Defense Rate Increase", damage: 12, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(302, "Maximum SD increase", damage: 13, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(303, "Auto Mana Recovery Increase", damage: 7, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(304, "Poison Resistance Increase", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, elementalModifier: (ElementalType)6, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, illusionKnightClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(305, "Durability Reduction (2)", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(306, "SD Recovery Speed Increase", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(307, "Automatic HP Recovery Increase", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(308, "Lightning Resistance Increase", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, elementalModifier: (ElementalType)6, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, illusionKnightClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(309, "Defense Increase", damage: 16, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3);
        this.CreateSkill(310, "Automatic AG Recovery Increase", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(311, "Ice Resistance Increase", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, illusionKnightClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(312, "Durability Reduction (3)", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(313, "Defense Success Rate Increase", damage: 1, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(325, "Attack Success Rate Increase", damage: 13, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(347, "Attack Rate", damage: 14, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(746, "Elemental DEF Increase", damage: 162, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, ragefighterClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(315, "Armor Set Bonus Increase", damage: 3, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(316, "Vengeance", damage: 38, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, gunCrusherClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(317, "Energy Increase", damage: 40, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(318, "Stamina Increase", damage: 40, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(319, "Agility Increase", damage: 40, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(320, "Strength Increase", damage: 40, distance: 0, abilityConsumption: 0, manaConsumption: 0, wizardClass: 3, knightClass: 3, elfClass: 3, magicGladiatorClass: 3, darkLordClass: 3, summonerClass: 3, growLancerClass: 3, runeWizardClass: 3, slayerClass: 3, whiteWizardClass: 3, lemuriaClass: 3, illusionKnightClass: 3, alchemistClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);

        // GrandMaster
        this.AddMasterSkillDefinition(300, 0, 0, 1, 1, 0, 20, Formulas[37], Formulas[37], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(301, 0, 0, 1, 1, 0, 20, Formulas[12], Formulas[12], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(302, 0, 0, 1, 2, 0, 20, Formulas[13], Formulas[13], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(303, 0, 0, 1, 2, 0, 20, Formulas[7], Formulas[7], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(305, 300, 0, 1, 3, 300, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(306, 302, 0, 1, 3, 302, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(307, 303, 0, 1, 3, 303, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(309, 0, 0, 1, 4, 0, 20, Formulas[16], Formulas[16], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(310, 307, 0, 1, 4, 307, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(312, 305, 0, 1, 5, 305, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(313, 309, 0, 1, 5, 309, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(315, 0, 0, 1, 6, 0, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(316, 315, 0, 1, 6, 315, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(317, 0, 0, 1, 7, 0, 20, Formulas[40], Formulas[40], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(318, 0, 0, 1, 7, 0, 20, Formulas[40], Formulas[40], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(319, 0, 0, 1, 7, 0, 20, Formulas[40], Formulas[40], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(320, 0, 0, 1, 7, 0, 20, Formulas[40], Formulas[40], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(322, 0, 0, 1, 8, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(324, 322, 0, 1, 9, 322, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(325, 0, 0, 2, 1, 0, 20, Formulas[13], Formulas[13], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(334, 0, 0, 2, 4, 0, 20, Formulas[9], Formulas[9], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(335, 0, 0, 2, 3, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(336, 0, 0, 2, 4, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(338, 334, 0, 2, 5, 334, 20, Formulas[9], Formulas[9], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(339, 336, 0, 2, 5, 336, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(341, 338, 0, 2, 6, 338, 20, Formulas[8], Formulas[8], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(344, 0, 0, 2, 8, 0, 10, Formulas[25], Formulas[25], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(345, 0, 0, 2, 6, 0, 20, Formulas[7], Formulas[7], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(346, 344, 0, 2, 9, 344, 20, Formulas[5], Formulas[5], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(347, 0, 0, 3, 1, 0, 20, Formulas[14], Formulas[14], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(348, 0, 0, 3, 2, 0, 20, Formulas[42], Formulas[42], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(349, 0, 0, 3, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(350, 0, 0, 3, 2, 0, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(351, 0, 0, 3, 2, 0, 20, Formulas[41], Formulas[41], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(352, 348, 0, 3, 3, 348, 20, Formulas[41], Formulas[41], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(353, 349, 0, 3, 3, 349, 10, Formulas[23], Formulas[23], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(354, 350, 0, 3, 3, 350, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(355, 351, 0, 3, 3, 351, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(356, 0, 0, 3, 4, 0, 20, Formulas[7], Formulas[7], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(357, 0, 0, 3, 4, 0, 20, Formulas[18], Formulas[18], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(358, 0, 0, 3, 4, 0, 20, Formulas[11], Formulas[11], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(359, 0, 0, 3, 4, 0, 20, Formulas[6], Formulas[6], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(360, 356, 0, 3, 5, 356, 20, Formulas[7], Formulas[7], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(361, 0, 0, 3, 5, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(362, 359, 0, 3, 5, 359, 20, Formulas[6], Formulas[6], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(363, 360, 0, 3, 6, 360, 20, Formulas[7], Formulas[7], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(364, 361, 0, 3, 6, 361, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(366, 0, 0, 3, 6, 0, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(367, 0, 0, 3, 7, 0, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(368, 0, 0, 3, 7, 0, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(369, 366, 0, 3, 7, 366, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(370, 0, 0, 3, 8, 0, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(371, 370, 0, 3, 9, 370, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(372, 368, 0, 3, 8, 368, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(626, 0, 0, 1, 9, 0, 20, Formulas[35], Formulas[35], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(628, 0, 0, 3, 7, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(629, 628, 0, 3, 9, 628, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(630, 0, 0, 3, 8, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(743, 0, 0, 2, 7, 0, 20, Formulas[43], Formulas[43], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(746, 0, 0, 1, 4, 0, 10, Formulas[162], Formulas[162], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(802, 0, 0, 2, 2, 0, 10, Formulas[227], Formulas[227], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(803, 0, 0, 2, 3, 0, 20, Formulas[228], Formulas[228], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(804, 803, 0, 2, 4, 803, 20, Formulas[229], Formulas[229], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(806, 804, 0, 2, 5, 804, 20, Formulas[231], Formulas[231], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(807, 806, 0, 2, 6, 806, 10, Formulas[232], Formulas[232], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(809, 807, 0, 2, 7, 807, 20, Formulas[234], Formulas[234], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(810, 0, 0, 2, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(811, 0, 0, 2, 2, 0, 20, Formulas[40], Formulas[40], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(812, 0, 0, 2, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(813, 0, 0, 2, 7, 0, 10, Formulas[178], Formulas[178], null, AggregateType.AddRaw);

        // MasterSlayer
        this.AddMasterSkillDefinition(326, 0, 0, 2, 2, 0, 20, Formulas[4], Formulas[4], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(330, 0, 0, 2, 3, 0, 20, Formulas[40], Formulas[40], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(331, 0, 0, 2, 3, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(631, 0, 0, 2, 8, 0, 10, Formulas[178], Formulas[178], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(779, 0, 0, 2, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(780, 779, 0, 2, 3, 779, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(781, 0, 0, 2, 6, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(782, 781, 0, 2, 7, 781, 10, Formulas[23], Formulas[23], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(783, 0, 0, 3, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(784, 783, 0, 3, 3, 783, 10, Formulas[23], Formulas[23], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(785, 0, 0, 1, 8, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(786, 785, 0, 1, 9, 785, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(787, 0, 0, 2, 8, 0, 20, Formulas[23], Formulas[23], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(788, 787, 0, 2, 9, 787, 20, Formulas[214], Formulas[214], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(789, 0, 0, 2, 4, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(790, 0, 0, 1, 9, 0, 20, Formulas[35], Formulas[35], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(791, 0, 0, 1, 8, 0, 20, Formulas[2], Formulas[2], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(792, 0, 0, 3, 7, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(793, 792, 0, 3, 9, 792, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(794, 0, 0, 2, 8, 0, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);

        // MirageLancer
        this.AddMasterSkillDefinition(329, 0, 0, 2, 2, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(685, 0, 0, 1, 8, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(686, 685, 0, 1, 9, 685, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(687, 0, 0, 2, 2, 0, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(688, 0, 0, 2, 2, 0, 20, Formulas[4], Formulas[4], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(689, 0, 0, 2, 3, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(690, 687, 0, 2, 4, 687, 20, Formulas[4], Formulas[4], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(691, 688, 0, 2, 4, 688, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(692, 0, 0, 2, 5, 0, 20, Formulas[4], Formulas[4], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(693, 0, 0, 2, 4, 0, 20, Formulas[18], Formulas[18], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(695, 692, 0, 2, 6, 692, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(696, 0, 0, 2, 5, 0, 20, Formulas[4], Formulas[4], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(698, 696, 0, 2, 6, 696, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(699, 698, 0, 2, 6, 698, 20, Formulas[116], Formulas[116], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(700, 0, 0, 2, 8, 0, 10, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(701, 700, 0, 2, 9, 700, 20, Formulas[23], Formulas[23], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(702, 0, 0, 3, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(703, 0, 0, 3, 2, 0, 20, Formulas[10], Formulas[10], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(704, 0, 0, 3, 2, 0, 20, Formulas[10], Formulas[10], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(705, 702, 0, 3, 3, 702, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(706, 703, 0, 3, 3, 703, 20, Formulas[23], Formulas[23], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(707, 704, 0, 3, 3, 704, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(711, 0, 0, 3, 9, 0, 10, Formulas[23], Formulas[23], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(712, 0, 0, 3, 9, 0, 10, Formulas[23], Formulas[23], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(713, 0, 0, 1, 8, 0, 20, Formulas[2], Formulas[2], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(715, 713, 0, 1, 9, 713, 20, Formulas[39], Formulas[39], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(716, 0, 0, 1, 9, 0, 20, Formulas[35], Formulas[35], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(718, 0, 0, 3, 7, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(719, 718, 0, 3, 9, 718, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(894, 695, 0, 2, 7, 695, 20, Formulas[313], Formulas[313], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(895, 0, 0, 3, 4, 0, 10, Formulas[312], Formulas[312], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(896, 698, 0, 2, 7, 698, 20, Formulas[315], Formulas[315], null, AggregateType.AddRaw);

        // BladeMaster
        this.AddMasterSkillDefinition(375, 0, 0, 1, 8, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(377, 375, 0, 1, 9, 375, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(378, 0, 0, 2, 2, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(379, 0, 0, 2, 2, 0, 20, Formulas[40], Formulas[40], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(380, 0, 0, 2, 2, 0, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(381, 378, 0, 2, 3, 378, 20, Formulas[4], Formulas[4], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(382, 379, 0, 2, 3, 379, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(383, 380, 0, 2, 3, 380, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(385, 0, 0, 2, 4, 0, 20, Formulas[4], Formulas[4], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(386, 385, 0, 2, 4, 385, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(387, 0, 0, 2, 4, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(388, 0, 0, 2, 5, 0, 20, Formulas[40], Formulas[40], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(389, 0, 0, 2, 5, 0, 20, Formulas[40], Formulas[40], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(390, 0, 0, 2, 6, 0, 20, Formulas[40], Formulas[40], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(391, 389, 0, 2, 6, 389, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(392, 388, 0, 2, 7, 388, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(397, 0, 0, 3, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(398, 0, 0, 3, 2, 0, 20, Formulas[42], Formulas[42], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(399, 0, 0, 3, 2, 0, 20, Formulas[10], Formulas[10], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(400, 397, 0, 3, 3, 397, 10, Formulas[23], Formulas[23], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(401, 398, 0, 3, 3, 398, 20, Formulas[42], Formulas[42], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(402, 399, 0, 3, 3, 399, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(403, 0, 0, 3, 4, 0, 20, Formulas[7], Formulas[7], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(404, 403, 0, 3, 5, 403, 20, Formulas[10], Formulas[10], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(405, 0, 0, 3, 5, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(406, 404, 0, 3, 6, 404, 20, Formulas[7], Formulas[7], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(407, 405, 0, 3, 6, 405, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(495, 0, 0, 2, 8, 0, 10, Formulas[26], Formulas[26], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(497, 495, 0, 2, 9, 495, 20, Formulas[4], Formulas[4], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(634, 0, 0, 1, 8, 0, 20, Formulas[2], Formulas[2], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(636, 634, 0, 1, 9, 634, 20, Formulas[39], Formulas[39], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(637, 0, 0, 1, 9, 0, 20, Formulas[35], Formulas[35], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(639, 0, 0, 3, 7, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(640, 639, 0, 3, 9, 639, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(641, 0, 0, 3, 8, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(642, 0, 0, 2, 9, 0, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);

        // HighElf
        this.AddMasterSkillDefinition(410, 0, 0, 1, 8, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(411, 0, 0, 2, 6, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(412, 410, 0, 1, 9, 410, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(413, 0, 0, 2, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(414, 0, 0, 2, 2, 0, 20, Formulas[4], Formulas[4], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(415, 0, 0, 2, 2, 0, 20, Formulas[16], Formulas[16], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(416, 0, 0, 2, 3, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(417, 0, 0, 2, 3, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(418, 414, 0, 2, 3, 414, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(419, 415, 0, 2, 3, 415, 20, Formulas[16], Formulas[16], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(420, 0, 0, 2, 4, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(421, 0, 0, 2, 4, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(422, 420, 0, 2, 5, 420, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(423, 417, 0, 2, 5, 417, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(424, 0, 0, 2, 5, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(425, 423, 0, 2, 6, 423, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(426, 0, 0, 2, 7, 0, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(427, 0, 0, 2, 8, 0, 10, Formulas[27], Formulas[27], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(428, 0, 0, 2, 7, 0, 20, Formulas[16], Formulas[16], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(429, 426, 0, 2, 8, 426, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(430, 0, 0, 2, 8, 0, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(432, 428, 0, 2, 8, 428, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(433, 430, 0, 2, 9, 430, 20, Formulas[10], Formulas[10], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(434, 427, 0, 2, 9, 427, 20, Formulas[40], Formulas[40], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(435, 0, 0, 3, 2, 0, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(436, 0, 0, 3, 2, 0, 20, Formulas[4], Formulas[4], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(437, 0, 0, 3, 2, 0, 20, Formulas[10], Formulas[10], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(438, 435, 0, 3, 3, 435, 10, Formulas[23], Formulas[23], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(439, 436, 0, 3, 3, 436, 20, Formulas[5], Formulas[5], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(440, 437, 0, 3, 3, 437, 20, Formulas[15], Formulas[15], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(441, 0, 0, 3, 5, 0, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(442, 0, 0, 3, 5, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(443, 442, 0, 3, 6, 442, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(643, 0, 0, 1, 8, 0, 20, Formulas[2], Formulas[2], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(645, 643, 0, 1, 9, 643, 20, Formulas[39], Formulas[39], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(646, 0, 0, 1, 9, 0, 20, Formulas[35], Formulas[35], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(648, 0, 0, 3, 7, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(649, 648, 0, 3, 9, 648, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(650, 0, 0, 3, 8, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(651, 0, 0, 2, 9, 0, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(652, 0, 0, 2, 9, 0, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(876, 0, 0, 2, 6, 0, 20, Formulas[41], Formulas[41], null, AggregateType.AddRaw);

        // DimensionMaster
        this.AddMasterSkillDefinition(446, 0, 0, 1, 8, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(447, 446, 0, 1, 9, 446, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(454, 0, 0, 2, 3, 0, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(455, 0, 0, 2, 3, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(456, 0, 0, 2, 4, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(457, 0, 0, 2, 5, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(458, 0, 0, 2, 5, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(459, 0, 0, 2, 6, 0, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(460, 0, 0, 2, 6, 0, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(461, 0, 0, 2, 8, 0, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(465, 0, 0, 3, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(466, 0, 0, 3, 2, 0, 20, Formulas[4], Formulas[4], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(467, 465, 0, 3, 3, 465, 20, Formulas[5], Formulas[5], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(468, 466, 0, 3, 3, 466, 10, Formulas[23], Formulas[23], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(469, 0, 0, 3, 5, 0, 20, Formulas[172], Formulas[172], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(470, 469, 0, 3, 6, 469, 10, Formulas[173], Formulas[173], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(471, 0, 0, 3, 5, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(473, 471, 0, 3, 6, 471, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(656, 0, 0, 1, 9, 0, 20, Formulas[35], Formulas[35], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(658, 0, 0, 3, 7, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(659, 658, 0, 3, 9, 658, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(660, 0, 0, 3, 8, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(770, 0, 0, 3, 5, 0, 20, Formulas[174], Formulas[174], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(771, 770, 0, 3, 6, 770, 20, Formulas[175], Formulas[175], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(772, 459, 0, 2, 7, 459, 20, Formulas[177], Formulas[177], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(773, 460, 0, 2, 7, 460, 20, Formulas[177], Formulas[177], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(774, 0, 0, 2, 2, 0, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(775, 0, 0, 2, 2, 0, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(776, 0, 0, 2, 2, 0, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(777, 776, 0, 2, 3, 776, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(778, 777, 0, 2, 4, 777, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);

        // DuelMaster
        this.AddMasterSkillDefinition(476, 0, 0, 1, 8, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(478, 476, 0, 1, 9, 476, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(479, 0, 0, 2, 2, 0, 20, Formulas[4], Formulas[4], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(480, 0, 0, 2, 2, 0, 20, Formulas[40], Formulas[40], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(481, 0, 0, 2, 2, 0, 20, Formulas[40], Formulas[40], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(482, 0, 0, 2, 2, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(483, 0, 0, 2, 3, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(484, 480, 0, 2, 3, 480, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(485, 482, 0, 2, 3, 482, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(487, 0, 0, 2, 4, 0, 20, Formulas[4], Formulas[4], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(488, 487, 0, 2, 4, 487, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(490, 0, 0, 2, 4, 0, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(493, 490, 0, 2, 5, 490, 20, Formulas[7], Formulas[7], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(496, 0, 0, 2, 5, 0, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(663, 0, 0, 1, 8, 0, 20, Formulas[2], Formulas[2], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(664, 0, 0, 1, 8, 0, 20, Formulas[2], Formulas[2], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(665, 663, 0, 1, 9, 663, 20, Formulas[39], Formulas[39], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(666, 0, 0, 1, 9, 0, 20, Formulas[35], Formulas[35], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(668, 0, 0, 3, 7, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(669, 668, 0, 3, 9, 668, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(865, 0, 0, 2, 6, 0, 20, Formulas[275], Formulas[275], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(866, 865, 0, 2, 7, 865, 10, Formulas[278], Formulas[278], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(867, 0, 0, 2, 6, 0, 20, Formulas[276], Formulas[276], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(868, 867, 0, 2, 7, 867, 10, Formulas[279], Formulas[279], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(869, 0, 0, 2, 6, 0, 20, Formulas[277], Formulas[277], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(870, 869, 0, 2, 7, 869, 10, Formulas[280], Formulas[280], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(871, 0, 0, 3, 2, 0, 20, Formulas[281], Formulas[281], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(872, 0, 0, 3, 2, 0, 20, Formulas[282], Formulas[282], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(873, 871, 0, 3, 3, 871, 10, Formulas[283], Formulas[283], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(874, 872, 0, 3, 3, 872, 10, Formulas[284], Formulas[284], null, AggregateType.AddRaw);

        // LordEmperor
        this.CreateSkill(505, "Emperor Cape Defense PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(506, "Adds Command Stat", damage: 40, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(507, "Emperor Cape Attack PowUp", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(670, "Protection Shield", damage: 2, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(672, "Shield Block", damage: 39, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(673, "Steel Armor", damage: 35, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3, crusaderClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(508, "Fire Burst Strengthener", DamageType.Physical, damage: 4, distance: 6, abilityConsumption: 0, manaConsumption: 13, energyRequirement: 79, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 80), darkLordClass: 3);
        this.CreateSkill(518, "Fire Scream Strengthener", DamageType.Physical, damage: 22, distance: 6, abilityConsumption: 5, manaConsumption: 14, energyRequirement: 150, leadershipRequirement: 70, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), darkLordClass: 3);
        this.CreateSkill(519, "Electric Spark Strengthener", DamageType.Physical, damage: 3, distance: 10, abilityConsumption: 35, manaConsumption: 10, energyRequirement: 126, leadershipRequirement: 340, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), darkLordClass: 3);
        this.CreateSkill(523, "Chaotic Diseier Strengthener", DamageType.Physical, damage: 22, distance: 6, abilityConsumption: 8, manaConsumption: 15, energyRequirement: 84, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 80), darkLordClass: 3);
        this.CreateSkill(520, "Fire Scream Mastery", damage: 5, distance: 6, abilityConsumption: 5, manaConsumption: 14, energyRequirement: 150, leadershipRequirement: 70, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalStrength, 100), darkLordClass: 3);
        this.CreateSkill(513, "Weapon Mastery", DamageType.Physical, damage: 22, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3, skillType: SkillType.PassiveBoost);
        this.CreateSkill(911, "Spirit Blast Strengthen", DamageType.Physical, damage: 317, distance: 6, abilityConsumption: 16, manaConsumption: 35, leadershipRequirement: 620, strengthRequirement: 800, elementalModifier: (ElementalType)6, scalingStat1: (Stats.TotalLeadership, 50), scalingStat2: (Stats.TotalStrength, 100), darkLordClass: 3);
        this.CreateSkill(510, "Horse Strengthener", damage: 17, distance: 0, abilityConsumption: 0, manaConsumption: 0, darkLordClass: 3);

        this.AddMasterSkillDefinition(505, 0, 0, 1, 8, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(506, 0, 0, 1, 6, 0, 20, Formulas[40], Formulas[40], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(507, 505, 0, 1, 9, 505, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(508, 0, 0, 2, 2, 0, 20, Formulas[4], Formulas[4], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(510, 0, 0, 2, 4, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(511, 0, 0, 3, 5, 0, 10, Formulas[321], Formulas[321], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(512, 510, 0, 2, 5, 510, 20, Formulas[4], Formulas[4], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(513, 0, 0, 2, 3, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(518, 0, 0, 2, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(519, 0, 0, 2, 2, 0, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(520, 518, 0, 2, 3, 518, 20, Formulas[5], Formulas[5], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(523, 0, 0, 2, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(527, 0, 0, 3, 2, 0, 20, Formulas[305], Formulas[305], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(528, 0, 0, 3, 2, 0, 20, Formulas[10], Formulas[10], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(530, 0, 0, 3, 3, 0, 20, Formulas[7], Formulas[7], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(531, 527, 0, 3, 3, 527, 20, Formulas[5], Formulas[5], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(532, 528, 0, 3, 3, 528, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(533, 0, 0, 3, 2, 0, 10, Formulas[20], Formulas[20], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(534, 530, 0, 3, 4, 530, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(538, 0, 0, 3, 8, 0, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(670, 0, 0, 1, 8, 0, 20, Formulas[2], Formulas[2], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(672, 670, 0, 1, 9, 670, 20, Formulas[39], Formulas[39], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(673, 0, 0, 1, 9, 0, 20, Formulas[35], Formulas[35], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(675, 0, 0, 3, 7, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(676, 675, 0, 3, 9, 675, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(911, 508, 0, 2, 4, 508, 20, Formulas[317], Formulas[317], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(912, 0, 0, 2, 8, 0, 10, Formulas[318], Formulas[318], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(913, 0, 0, 3, 6, 0, 20, Formulas[319], Formulas[319], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(914, 0, 0, 2, 6, 0, 20, Formulas[320], Formulas[320], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(915, 0, 0, 2, 5, 0, 10, Formulas[323], Formulas[323], null, AggregateType.AddRaw);

        // FistMaster
        this.AddMasterSkillDefinition(549, 0, 0, 1, 8, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(550, 549, 0, 1, 9, 549, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(551, 0, 0, 2, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(552, 0, 0, 2, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(554, 551, 0, 2, 3, 551, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(555, 552, 0, 2, 3, 552, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(557, 0, 0, 2, 4, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(558, 0, 0, 2, 5, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(559, 0, 0, 2, 5, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(560, 0, 0, 2, 5, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(563, 559, 0, 2, 7, 559, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(564, 0, 0, 2, 7, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(565, 0, 0, 2, 8, 0, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(567, 565, 0, 2, 9, 565, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(568, 0, 0, 3, 2, 0, 20, Formulas[305], Formulas[305], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(569, 0, 0, 3, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(571, 568, 0, 3, 3, 568, 10, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(572, 569, 0, 3, 3, 569, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(573, 0, 0, 3, 4, 0, 20, Formulas[5], Formulas[5], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(578, 0, 0, 1, 1, 0, 20, Formulas[37], Formulas[37], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(579, 0, 0, 1, 1, 0, 20, Formulas[29], Formulas[29], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(580, 0, 0, 1, 2, 0, 20, Formulas[33], Formulas[33], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(581, 0, 0, 1, 2, 0, 20, Formulas[7], Formulas[7], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(583, 578, 0, 1, 3, 578, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(584, 580, 0, 1, 3, 580, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(585, 581, 0, 1, 3, 581, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(587, 0, 0, 1, 4, 0, 20, Formulas[35], Formulas[35], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(588, 585, 0, 1, 4, 585, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(590, 583, 0, 1, 5, 583, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(591, 587, 0, 1, 5, 587, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(593, 0, 0, 1, 6, 0, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(594, 593, 0, 1, 6, 593, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(595, 0, 0, 1, 7, 0, 20, Formulas[36], Formulas[36], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(596, 0, 0, 1, 7, 0, 20, Formulas[36], Formulas[36], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(597, 0, 0, 1, 7, 0, 20, Formulas[36], Formulas[36], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(598, 0, 0, 1, 7, 0, 20, Formulas[36], Formulas[36], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(599, 0, 0, 2, 1, 0, 20, Formulas[13], Formulas[13], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(600, 0, 0, 2, 4, 0, 20, Formulas[34], Formulas[34], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(601, 600, 0, 2, 5, 600, 20, Formulas[34], Formulas[34], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(602, 601, 0, 2, 6, 601, 20, Formulas[37], Formulas[37], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(603, 0, 0, 3, 1, 0, 20, Formulas[31], Formulas[31], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(604, 0, 0, 3, 4, 0, 20, Formulas[18], Formulas[18], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(605, 0, 0, 3, 4, 0, 20, Formulas[11], Formulas[11], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(606, 0, 0, 3, 4, 0, 20, Formulas[6], Formulas[6], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(607, 0, 0, 3, 5, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(608, 606, 0, 3, 5, 606, 20, Formulas[6], Formulas[6], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(609, 607, 0, 3, 6, 607, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(610, 0, 0, 3, 6, 0, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(611, 0, 0, 3, 7, 0, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(612, 0, 0, 3, 7, 0, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(613, 610, 0, 3, 7, 610, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(614, 0, 0, 3, 8, 0, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(615, 614, 0, 3, 9, 614, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(616, 612, 0, 3, 8, 612, 20, Formulas[38], Formulas[38], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(680, 0, 0, 1, 9, 0, 20, Formulas[35], Formulas[35], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(682, 0, 0, 3, 7, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(683, 682, 0, 3, 9, 682, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(744, 0, 0, 2, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(745, 744, 0, 2, 3, 744, 20, Formulas[1], Formulas[1], null, AggregateType.AddRaw);

        // GrandRuneMaster
        this.AddMasterSkillDefinition(748, 0, 0, 2, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(749, 748, 0, 2, 3, 748, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(750, 0, 0, 2, 6, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(751, 750, 0, 2, 7, 750, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(752, 0, 0, 3, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(753, 752, 0, 3, 3, 752, 10, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(754, 0, 0, 1, 8, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(755, 754, 0, 1, 9, 754, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(756, 758, 0, 1, 9, 758, 20, Formulas[39], Formulas[39], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(758, 0, 0, 1, 8, 0, 20, Formulas[2], Formulas[2], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(759, 0, 0, 1, 9, 0, 20, Formulas[35], Formulas[35], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(761, 0, 0, 3, 7, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(762, 761, 0, 3, 9, 761, 20, Formulas[3], Formulas[3], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(763, 0, 0, 3, 8, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(765, 0, 0, 2, 4, 0, 20, Formulas[23], Formulas[23], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(766, 765, 0, 2, 5, 765, 20, Formulas[243], Formulas[243], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(768, 0, 0, 2, 4, 0, 20, Formulas[23], Formulas[23], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(769, 768, 0, 2, 5, 768, 20, Formulas[243], Formulas[243], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(916, 0, 0, 2, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);

        // MasterGunBreaker
        this.AddMasterSkillDefinition(814, 0, 0, 1, 7, 0, 20, Formulas[245], Formulas[245], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(815, 0, 0, 1, 7, 0, 20, Formulas[246], Formulas[246], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(816, 0, 0, 1, 7, 0, 20, Formulas[247], Formulas[247], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(817, 0, 0, 1, 7, 0, 20, Formulas[248], Formulas[248], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(818, 0, 0, 1, 8, 0, 20, Formulas[249], Formulas[249], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(819, 818, 0, 1, 9, 818, 20, Formulas[250], Formulas[250], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(820, 0, 0, 2, 5, 0, 10, Formulas[251], Formulas[251], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(821, 820, 0, 2, 6, 820, 20, Formulas[252], Formulas[252], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(822, 821, 0, 2, 7, 821, 20, Formulas[253], Formulas[253], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(823, 0, 0, 2, 2, 0, 20, Formulas[254], Formulas[254], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(824, 823, 0, 2, 3, 823, 20, Formulas[255], Formulas[255], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(825, 0, 0, 2, 2, 0, 20, Formulas[256], Formulas[256], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(826, 825, 0, 2, 3, 825, 20, Formulas[257], Formulas[257], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(828, 0, 0, 2, 5, 0, 20, Formulas[285], Formulas[285], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(829, 828, 0, 2, 6, 828, 10, Formulas[286], Formulas[286], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(831, 0, 0, 3, 2, 0, 20, Formulas[262], Formulas[262], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(832, 831, 0, 3, 3, 831, 10, Formulas[263], Formulas[263], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(835, 0, 0, 2, 2, 0, 20, Formulas[269], Formulas[269], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(836, 835, 0, 2, 3, 835, 20, Formulas[270], Formulas[270], null, AggregateType.AddRaw);

        // ShineWizard
        this.AddMasterSkillDefinition(837, 0, 0, 1, 8, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(838, 837, 0, 1, 9, 837, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(839, 0, 0, 1, 9, 0, 20, Formulas[35], Formulas[35], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(843, 0, 0, 2, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(844, 843, 0, 2, 3, 843, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(845, 0, 0, 2, 4, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(846, 0, 0, 2, 6, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(847, 846, 0, 2, 7, 846, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(859, 0, 0, 3, 2, 0, 10, Formulas[23], Formulas[23], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(860, 859, 0, 3, 3, 859, 10, Formulas[1], Formulas[1], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(861, 0, 0, 3, 4, 0, 20, Formulas[7], Formulas[7], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(862, 861, 0, 3, 5, 861, 20, Formulas[10], Formulas[10], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(863, 862, 0, 3, 6, 862, 20, Formulas[7], Formulas[7], null, AggregateType.AddRaw);

        // ArchMage
        this.AddMasterSkillDefinition(840, 0, 0, 1, 8, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(841, 840, 0, 1, 9, 840, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(842, 0, 0, 1, 9, 0, 20, Formulas[35], Formulas[35], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(848, 0, 0, 2, 2, 0, 20, Formulas[4], Formulas[4], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(849, 848, 0, 2, 3, 848, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(850, 0, 0, 2, 4, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(851, 0, 0, 2, 3, 0, 20, Formulas[271], Formulas[271], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(852, 851, 0, 2, 4, 851, 20, Formulas[271], Formulas[271], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(853, 0, 0, 2, 4, 0, 20, Formulas[271], Formulas[271], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(854, 853, 0, 2, 5, 853, 20, Formulas[271], Formulas[271], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(855, 0, 0, 2, 6, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(856, 855, 0, 2, 7, 855, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(857, 0, 0, 2, 8, 0, 20, Formulas[272], Formulas[272], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(858, 0, 0, 2, 2, 0, 20, Formulas[271], Formulas[271], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(864, 0, 0, 3, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(875, 864, 0, 3, 3, 864, 10, Formulas[23], Formulas[23], null, AggregateType.AddRaw);

        // IllusionMaster
        this.AddMasterSkillDefinition(877, 0, 0, 2, 2, 0, 20, Formulas[294], Formulas[294], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(878, 877, 0, 2, 3, 877, 10, Formulas[295], Formulas[295], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(879, 0, 0, 2, 3, 0, 20, Formulas[296], Formulas[296], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(880, 879, 0, 2, 4, 879, 10, Formulas[297], Formulas[297], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(881, 880, 0, 2, 7, 880, 20, Formulas[298], Formulas[298], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(882, 881, 0, 2, 8, 881, 10, Formulas[299], Formulas[299], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(883, 0, 0, 2, 5, 0, 10, Formulas[300], Formulas[300], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(884, 883, 0, 2, 6, 883, 20, Formulas[301], Formulas[301], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(885, 0, 0, 2, 4, 0, 20, Formulas[302], Formulas[302], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(886, 885, 0, 2, 5, 885, 10, Formulas[303], Formulas[303], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(887, 886, 0, 2, 6, 886, 10, Formulas[304], Formulas[304], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(888, 0, 0, 1, 8, 0, 20, Formulas[2], Formulas[2], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(889, 0, 0, 1, 9, 0, 20, Formulas[35], Formulas[35], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(890, 0, 0, 1, 8, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(891, 890, 0, 1, 9, 890, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(892, 0, 0, 3, 2, 0, 20, Formulas[305], Formulas[305], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(893, 892, 0, 3, 3, 892, 10, Formulas[306], Formulas[306], null, AggregateType.AddRaw);

        // AlchemicMaster
        this.AddMasterSkillDefinition(897, 0, 0, 1, 8, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(898, 897, 0, 1, 9, 897, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(899, 0, 0, 1, 9, 0, 20, Formulas[35], Formulas[35], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(900, 0, 0, 2, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(901, 900, 0, 2, 3, 900, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(902, 0, 0, 2, 4, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(903, 0, 0, 2, 6, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(904, 903, 0, 2, 7, 903, 10, Formulas[0], Formulas[0], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(905, 0, 0, 2, 8, 0, 10, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(906, 905, 0, 2, 9, 905, 20, Formulas[23], Formulas[23], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(907, 0, 0, 3, 2, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(909, 0, 0, 3, 2, 0, 10, Formulas[23], Formulas[23], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(910, 909, 0, 3, 3, 909, 10, Formulas[1], Formulas[1], null, AggregateType.AddRaw);

        // MasterPaladin
        this.AddMasterSkillDefinition(917, 0, 0, 1, 8, 0, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(918, 917, 0, 1, 9, 917, 20, Formulas[17], Formulas[17], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(919, 0, 0, 2, 2, 0, 20, Formulas[326], Formulas[326], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(920, 919, 0, 2, 3, 919, 10, Formulas[327], Formulas[327], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(921, 0, 0, 2, 4, 0, 20, Formulas[324], Formulas[324], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(923, 0, 0, 2, 5, 0, 20, Formulas[328], Formulas[328], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(925, 0, 0, 2, 3, 0, 20, Formulas[22], Formulas[22], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(926, 0, 0, 3, 2, 0, 20, Formulas[330], Formulas[330], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(927, 926, 0, 3, 3, 926, 10, Formulas[331], Formulas[331], null, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(928, 0, 0, 3, 4, 0, 20, Formulas[332], Formulas[332], null, AggregateType.AddRaw);




        /*
        
        
        
        
        this.AddPassiveMasterSkillDefinition(304, Stats.PoisonResistance, AggregateType.AddRaw, Formula120, Formula120, 2, 1);
        this.AddPassiveMasterSkillDefinition(305, Stats.ItemDurationIncrease, AggregateType.Multiplicate, Formula1204, 3, 1, 300);
        this.AddMasterSkillDefinition(306, 302, 0, 1, 3, 0, 20, Formula120);
        this.AddPassiveMasterSkillDefinition(307, Stats.HealthRecoveryMultiplier, AggregateType.AddRaw, FormulaRecoveryIncrease120, Formula120, 3, 1, SkillNumber.AutomaticManaRecInc, SkillNumber.Undefined, 20);
        this.AddPassiveMasterSkillDefinition(308, Stats.LightningResistance, AggregateType.AddRaw, Formula120, Formula120, 2, 1, requiredSkill1: SkillNumber.PoisonResistanceInc);
        this.AddPassiveMasterSkillDefinition(309, Stats.DefenseBase, AggregateType.AddFinal, Formula6020, 4, 1);
        this.AddPassiveMasterSkillDefinition(310, Stats.AbilityRecoveryMultiplier, AggregateType.AddRaw, FormulaRecoveryIncrease120, Formula120, 4, 1, SkillNumber.AutomaticHpRecInc, SkillNumber.Undefined, 20);
        this.AddPassiveMasterSkillDefinition(311, Stats.IceResistance, AggregateType.AddRaw, Formula120, Formula120, 2, 1, requiredSkill1: SkillNumber.LightningResistanceInc);
        this.AddPassiveMasterSkillDefinition(312, Stats.ItemDurationIncrease, AggregateType.Multiplicate, Formula1204, 5, 1, SkillNumber.DurabilityReduction2);
        
        */
        /*
        // Universal
        

        // DK
        this.AddPassiveMasterSkillDefinition(SkillNumber.AttackSuccRateInc, Stats.AttackRatePvm, AggregateType.AddRaw, Formula51173, 1, 2);
        this.AddMasterSkillDefinition(SkillNumber.CycloneStrengthener, SkillNumber.Cyclone, SkillNumber.Undefined, 2, 2, SkillNumber.Cyclone, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.SlashStrengthener, SkillNumber.Slash, SkillNumber.Undefined, 2, 2, SkillNumber.Slash, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.FallingSlashStreng, SkillNumber.FallingSlash, SkillNumber.Undefined, 2, 2, SkillNumber.FallingSlash, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.LungeStrengthener, SkillNumber.Lunge, SkillNumber.Undefined, 2, 2, SkillNumber.Lunge, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.TwistingSlashStreng, SkillNumber.TwistingSlash, SkillNumber.Undefined, 2, 3, SkillNumber.TwistingSlash, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.RagefulBlowStreng, SkillNumber.RagefulBlow, SkillNumber.Undefined, 2, 3, SkillNumber.RagefulBlow, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.TwistingSlashMastery, SkillNumber.TwistingSlashStreng, SkillNumber.Undefined, 2, 4, SkillNumber.TwistingSlash, 20, Formula120);
        this.AddMasterSkillDefinition(SkillNumber.RagefulBlowMastery, SkillNumber.RagefulBlowStreng, SkillNumber.Undefined, 2, 4, SkillNumber.RagefulBlow, 20, Formula120);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MaximumLifeIncrease, Stats.MaximumHealth, AggregateType.AddRaw, Formula10235, 4, 2);
        this.AddPassiveMasterSkillDefinition(SkillNumber.WeaponMasteryBladeMaster, Stats.MasterSkillPhysBonusDmg, AggregateType.AddRaw, Formula502, 4, 2);
        this.AddMasterSkillDefinition(SkillNumber.DeathStabStrengthener, SkillNumber.DeathStab, SkillNumber.Undefined, 2, 5, SkillNumber.DeathStab, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.StrikeofDestrStr, SkillNumber.StrikeofDestruction, SkillNumber.Undefined, 2, 5, SkillNumber.StrikeofDestruction, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MaximumManaIncrease, Stats.MaximumMana, AggregateType.AddRaw, Formula10235, 5, 2, SkillNumber.MaximumLifeIncrease);
        this.AddPassiveMasterSkillDefinition(SkillNumber.PvPAttackRate, Stats.AttackRatePvp, AggregateType.AddRaw, Formula81877, 1, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.TwoHandedSwordStrengthener, Stats.TwoHandedSwordStrBonusDamage, AggregateType.AddRaw, Formula883, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.OneHandedSwordStrengthener, Stats.OneHandedSwordBonusDamage, AggregateType.AddRaw, Formula502, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MaceStrengthener, Stats.MaceBonusDamage, AggregateType.AddRaw, Formula632, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.SpearStrengthener, Stats.SpearBonusDamage, AggregateType.AddRaw, Formula632, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.TwoHandedSwordMaster, Stats.TwoHandedSwordMasteryBonusDamage, AggregateType.AddRaw, Formula1154, 3, 3, SkillNumber.TwoHandedSwordStrengthener);
        this.AddPassiveMasterSkillDefinition(SkillNumber.OneHandedSwordMaster, Stats.WeaponMasteryAttackSpeed, AggregateType.AddRaw, Formula1, 3, 3, SkillNumber.OneHandedSwordStrengthener, SkillNumber.Undefined, 10);

        // todo: Probability of stunning the target for 2 seconds according to the assigned Skill Level while using a Mace.
        this.AddMasterSkillDefinition(SkillNumber.MaceMastery, SkillNumber.MaceStrengthener, SkillNumber.Undefined, 3, 3, SkillNumber.Undefined, 20, Formula120);

        // todo: Increases the probability of Double Damage while using a Spear according to the assigned Skill Level.
        this.AddMasterSkillDefinition(SkillNumber.SpearMastery, SkillNumber.SpearStrengthener, SkillNumber.Undefined, 3, 3, SkillNumber.Undefined, 20, Formula120);

        this.AddMasterSkillDefinition(SkillNumber.SwellLifeStrengt, SkillNumber.SwellLife, SkillNumber.Undefined, 3, 4, SkillNumber.SwellLife, 20, Formula181);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ManaReduction, Stats.ManaUsageReduction, AggregateType.AddRaw, Formula722Value, Formula722,  4, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MonsterAttackSdInc, Stats.ShieldAfterMonsterKillMultiplier, AggregateType.AddFinal, Formula914, 4, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MonsterAttackLifeInc, Stats.HealthAfterMonsterKillMultiplier, AggregateType.AddFinal, Formula4319, 4, 3);
        this.AddMasterSkillDefinition(SkillNumber.SwellLifeProficiency, SkillNumber.SwellLifeStrengt, SkillNumber.Undefined, 3, 5, SkillNumber.SwellLife, 20, Formula181);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MinimumAttackPowerInc, Stats.MinimumPhysBaseDmg, AggregateType.AddRaw, Formula502, 5, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MonsterAttackManaInc, Stats.ManaAfterMonsterKillMultiplier, AggregateType.AddFinal, Formula4319, 5, 3, SkillNumber.MonsterAttackLifeInc);

        // DW
        this.AddMasterSkillDefinition(SkillNumber.FlameStrengthener, SkillNumber.Flame, SkillNumber.Undefined, 2, 2, SkillNumber.Flame, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.LightningStrengthener, SkillNumber.Lightning, SkillNumber.Undefined, 2, 2, SkillNumber.Lightning, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.ExpansionofWizStreng, SkillNumber.ExpansionofWizardry, SkillNumber.Undefined, 2, 2, SkillNumber.ExpansionofWizardry, 20, Formula120Value, Formula120, Stats.MaximumWizBaseDmg, AggregateType.Multiplicate);
        this.AddMasterSkillDefinition(SkillNumber.InfernoStrengthener, SkillNumber.Inferno, SkillNumber.FlameStrengthener, 2, 3, SkillNumber.Inferno, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.BlastStrengthener, SkillNumber.Cometfall, SkillNumber.LightningStrengthener, 2, 3, SkillNumber.Cometfall, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.ExpansionofWizMas, SkillNumber.ExpansionofWizStreng, SkillNumber.Undefined, 2, 3, SkillNumber.ExpansionofWizardry, 20, Formula120Value, Formula120, targetAttribute: Stats.CriticalDamageChance, AggregateType.Multiplicate);
        this.AddMasterSkillDefinition(SkillNumber.PoisonStrengthener, SkillNumber.Poison, SkillNumber.Undefined, 2, 3, SkillNumber.Poison, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.EvilSpiritStreng, SkillNumber.EvilSpirit, SkillNumber.Undefined, 2, 4, SkillNumber.EvilSpirit, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MagicMasteryGrandMaster, Stats.WizardryBaseDmg, AggregateType.AddRaw, Formula502, 4, 2, SkillNumber.EvilSpiritStreng);
        this.AddMasterSkillDefinition(SkillNumber.DecayStrengthener, SkillNumber.Decay, SkillNumber.PoisonStrengthener, 2, 4, SkillNumber.Decay, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.HellfireStrengthener, SkillNumber.Hellfire, SkillNumber.Undefined, 2, 5, SkillNumber.Hellfire, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.IceStrengthener, SkillNumber.Ice, SkillNumber.Undefined, 2, 5, SkillNumber.Ice, 20, Formula632);
        this.AddPassiveMasterSkillDefinition(SkillNumber.OneHandedStaffStrengthener, Stats.OneHandedStaffBonusBaseDamage, AggregateType.AddRaw, Formula502, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.TwoHandedStaffStrengthener, Stats.TwoHandedStaffBonusBaseDamage, AggregateType.AddRaw, Formula883, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ShieldStrengthenerGrandMaster, Stats.BonusDefenseWithShield, AggregateType.AddRaw, Formula803, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.OneHandedStaffMaster, Stats.WeaponMasteryAttackSpeed, AggregateType.AddRaw, Formula1, 3, 3, SkillNumber.OneHandedStaffStrengthener, SkillNumber.Undefined, 10);
        this.AddPassiveMasterSkillDefinition(SkillNumber.TwoHandedStaffMaster, Stats.TwoHandedStaffMasteryBonusDamage, AggregateType.AddRaw, Formula1154, 3, 3, SkillNumber.TwoHandedStaffStrengthener);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ShieldMasteryGrandMaster, Stats.BonusDefenseRateWithShield, AggregateType.AddRaw, Formula1204, 3, 3, SkillNumber.ShieldStrengthenerGrandMaster);
        this.AddMasterSkillDefinition(SkillNumber.SoulBarrierStrength, SkillNumber.SoulBarrier, SkillNumber.Undefined, 3, 4, SkillNumber.SoulBarrier, 20, $"{Formula181} / 100", Formula181, Stats.SoulBarrierReceiveDecrement, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(SkillNumber.SoulBarrierProficie, SkillNumber.SoulBarrierStrength, SkillNumber.Undefined, 3, 5, SkillNumber.SoulBarrierStrength, 20, Formula803, extendsDuration: true);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MinimumWizardryInc, Stats.MinimumWizBaseDmg, AggregateType.AddRaw, Formula502, 5, 3);

        // ELF
        this.AddMasterSkillDefinition(SkillNumber.HealStrengthener, SkillNumber.Heal, SkillNumber.Undefined, 2, 2, SkillNumber.Heal, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.TripleShotStrengthener, SkillNumber.TripleShot, SkillNumber.Undefined, 2, 2, SkillNumber.TripleShot, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.SummonedMonsterStr1, Stats.SummonedMonsterHealthIncrease, AggregateType.AddRaw, Formula6020Value, 2, 2, SkillNumber.SummonGoblin);
        this.AddMasterSkillDefinition(SkillNumber.PenetrationStrengthener, SkillNumber.Penetration, SkillNumber.Undefined, 2, 3, SkillNumber.Penetration, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.DefenseIncreaseStr, SkillNumber.GreaterDefense, SkillNumber.Undefined, 2, 3, SkillNumber.GreaterDefense, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.TripleShotMastery, SkillNumber.TripleShotStrengthener, SkillNumber.Undefined, 2, 3, SkillNumber.TripleShot, 10, Formula1WhenComplete);
        this.AddPassiveMasterSkillDefinition(SkillNumber.SummonedMonsterStr2, Stats.SummonedMonsterDefenseIncrease, AggregateType.AddRaw, Formula6020, 2, 3, SkillNumber.SummonGoblin);
        this.AddMasterSkillDefinition(SkillNumber.AttackIncreaseStr, SkillNumber.GreaterDamage, SkillNumber.Undefined, 2, 4, SkillNumber.GreaterDamage, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.WeaponMasteryHighElf, Stats.MasterSkillPhysBonusDmg, AggregateType.AddRaw, Formula502, 4, 2);
        this.AddMasterSkillDefinition(SkillNumber.AttackIncreaseMastery, SkillNumber.AttackIncreaseStr, SkillNumber.Undefined, 2, 5, SkillNumber.GreaterDamage, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.DefenseIncreaseMastery, SkillNumber.DefenseIncreaseStr, SkillNumber.Undefined, 2, 5, SkillNumber.GreaterDefense, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.IceArrowStrengthener, SkillNumber.IceArrow, SkillNumber.Undefined, 2, 5, SkillNumber.IceArrow, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.BowStrengthener, Stats.BowStrBonusDamage, AggregateType.AddRaw, Formula502, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.CrossbowStrengthener, Stats.CrossBowStrBonusDamage, AggregateType.AddRaw, Formula632, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ShieldStrengthenerHighElf, Stats.BonusDefenseWithShield, AggregateType.AddRaw, Formula803, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.BowMastery, Stats.WeaponMasteryAttackSpeed, AggregateType.AddRaw, Formula1, 3, 3, SkillNumber.BowStrengthener, SkillNumber.Undefined, 10);
        this.AddPassiveMasterSkillDefinition(SkillNumber.CrossbowMastery, Stats.CrossBowMasteryBonusDamage, AggregateType.AddRaw, Formula1154, 3, 3, SkillNumber.CrossbowStrengthener);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ShieldMasteryHighElf, Stats.BonusDefenseRateWithShield, AggregateType.AddRaw, Formula1806, 3, 3, SkillNumber.ShieldStrengthenerHighElf);
        this.AddMasterSkillDefinition(SkillNumber.InfinityArrowStr, SkillNumber.InfinityArrow, SkillNumber.Undefined, 3, 5, SkillNumber.InfinityArrow, 20, $"1 + {Formula120} / 100", Formula120, Stats.AttackDamageIncrease, AggregateType.Multiplicate);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MinimumAttPowerInc, Stats.MinimumPhysBaseDmg, AggregateType.AddRaw, Formula502, 5, 3);

        // SUM
        this.AddMasterSkillDefinition(SkillNumber.FireTomeStrengthener, SkillNumber.Undefined, SkillNumber.Undefined, 2, 2, SkillNumber.Undefined, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.WindTomeStrengthener, SkillNumber.Undefined, SkillNumber.Undefined, 2, 2, SkillNumber.Undefined, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.LightningTomeStren, SkillNumber.Undefined, SkillNumber.Undefined, 2, 2, SkillNumber.Undefined, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.FireTomeMastery, SkillNumber.FireTomeStrengthener, SkillNumber.Undefined, 2, 3, SkillNumber.Undefined, 20, Formula181);
        this.AddMasterSkillDefinition(SkillNumber.WindTomeMastery, SkillNumber.WindTomeStrengthener, SkillNumber.Undefined, 2, 3, SkillNumber.Undefined, 20, Formula120);
        this.AddMasterSkillDefinition(SkillNumber.LightningTomeMastery, SkillNumber.LightningTomeStren, SkillNumber.Undefined, 2, 3, SkillNumber.Undefined, 20, Formula181);
        this.AddMasterSkillDefinition(SkillNumber.SleepStrengthener, SkillNumber.Sleep, SkillNumber.Undefined, 2, 3, SkillNumber.Sleep, 20, Formula120);
        this.AddMasterSkillDefinition(SkillNumber.ChainLightningStr, SkillNumber.ChainLightning, SkillNumber.Undefined, 2, 4, SkillNumber.ChainLightning, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.LightningShockStr, SkillNumber.LightningShock, SkillNumber.Undefined, 2, 4, SkillNumber.LightningShock, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MagicMasterySummoner, Stats.WizardryAndCurseBaseDmgBonus, AggregateType.AddRaw, Formula502, 5, 2);
        this.AddMasterSkillDefinition(SkillNumber.DrainLifeStrengthener, SkillNumber.DrainLife, SkillNumber.Undefined, 2, 5, SkillNumber.DrainLife, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.StickStrengthener, Stats.StickBonusBaseDamage, AggregateType.AddRaw, Formula502, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.OtherWorldTomeStreng, Stats.BookBonusBaseDamage, AggregateType.AddRaw, Formula632, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.StickMastery, Stats.StickMasteryBonusDamage, AggregateType.AddRaw, Formula1154, 3, 3, SkillNumber.StickStrengthener);
        this.AddPassiveMasterSkillDefinition(SkillNumber.OtherWorldTomeMastery, Stats.WeaponMasteryAttackSpeed, AggregateType.AddRaw, Formula1, 3, 3, SkillNumber.OtherWorldTomeStreng, SkillNumber.Undefined, 10);
        this.AddMasterSkillDefinition(SkillNumber.BerserkerStrengthener, SkillNumber.Berserker, SkillNumber.Undefined, 3, 4, SkillNumber.Berserker, 20, $"{Formula181} / 100", Formula181, Stats.BerserkerCurseMultiplier, AggregateType.AddRaw);
        this.AddMasterSkillDefinition(SkillNumber.BerserkerProficiency, SkillNumber.BerserkerStrengthener, SkillNumber.Undefined, 3, 5, SkillNumber.BerserkerStrengthener, 20, $"{Formula181} / 100", Formula181, Stats.BerserkerProficiencyMultiplier, AggregateType.AddRaw);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MinimumWizCurseInc, Stats.MinWizardryAndCurseDmgBonus, AggregateType.AddRaw, Formula502, 5, 3);

        // MG
        this.AddMasterSkillDefinition(SkillNumber.CycloneStrengthenerDuelMaster, SkillNumber.Cyclone, SkillNumber.Undefined, 2, 2, SkillNumber.Cyclone, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.LightningStrengthenerDuelMaster, SkillNumber.Lightning, SkillNumber.Undefined, 2, 2, SkillNumber.Lightning, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.TwistingSlashStrengthenerDuelMaster, SkillNumber.TwistingSlash, SkillNumber.Undefined, 2, 2, SkillNumber.TwistingSlash, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.PowerSlashStreng, SkillNumber.PowerSlash, SkillNumber.Undefined, 2, 2, SkillNumber.PowerSlash, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.FlameStrengthenerDuelMaster, SkillNumber.Flame, SkillNumber.Undefined, 2, 3, SkillNumber.Flame, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.BlastStrengthenerDuelMaster, SkillNumber.Cometfall, SkillNumber.LightningStrengthenerDuelMaster, 2, 3, SkillNumber.Cometfall, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.WeaponMasteryDuelMaster, Stats.MasterSkillPhysBonusDmg, AggregateType.AddRaw, Formula502, 3, 2, SkillNumber.TwistingSlashStrengthenerDuelMaster, SkillNumber.PowerSlashStreng);
        this.AddMasterSkillDefinition(SkillNumber.InfernoStrengthenerDuelMaster, SkillNumber.Inferno, SkillNumber.FlameStrengthenerDuelMaster, 2, 4, SkillNumber.Inferno, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.EvilSpiritStrengthenerDuelMaster, SkillNumber.EvilSpirit, SkillNumber.Undefined, 2, 4, SkillNumber.EvilSpirit, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.MagicMasteryDuelMaster, Stats.WizardryBaseDmg, AggregateType.AddRaw, Formula502, 4, 2, SkillNumber.EvilSpiritStrengthenerDuelMaster);
        this.AddMasterSkillDefinition(SkillNumber.IceStrengthenerDuelMaster, SkillNumber.Ice, SkillNumber.Undefined, 2, 5, SkillNumber.Ice, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.BloodAttackStrengthen, SkillNumber.FireSlash, SkillNumber.Undefined, 2, 5, SkillNumber.FireSlash, 20, Formula502);

        // DL
        this.AddMasterSkillDefinition(SkillNumber.FireBurstStreng, SkillNumber.FireBurst, SkillNumber.Undefined, 2, 2, SkillNumber.FireBurst, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.ForceWaveStreng, SkillNumber.Force, SkillNumber.Undefined, 2, 2, SkillNumber.ForceWave, 20, Formula632);
        this.AddPassiveMasterSkillDefinition(SkillNumber.DarkHorseStreng1, Stats.BonusDefenseWithHorse, AggregateType.AddRaw, Formula1204, 2, 2);
        this.AddMasterSkillDefinition(SkillNumber.CriticalDmgIncPowUp, SkillNumber.IncreaseCriticalDamage, SkillNumber.Undefined, 2, 3, SkillNumber.IncreaseCriticalDamage, 20, Formula632);
        this.AddMasterSkillDefinition(SkillNumber.EarthshakeStreng, SkillNumber.Earthshake, SkillNumber.DarkHorseStreng1, 2, 3, SkillNumber.Earthshake, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.WeaponMasteryLordEmperor, Stats.MasterSkillPhysBonusDmg, AggregateType.AddRaw, Formula502, 3, 2);
        this.AddMasterSkillDefinition(SkillNumber.FireBurstMastery, SkillNumber.FireBurstStreng, SkillNumber.Undefined, 2, 4, SkillNumber.FireBurst, 20, Formula120);
        this.AddMasterSkillDefinition(SkillNumber.CritDmgIncPowUp2, SkillNumber.CriticalDmgIncPowUp, SkillNumber.Undefined, 2, 4, SkillNumber.IncreaseCriticalDamage, 20, Formula803);
        this.AddMasterSkillDefinition(SkillNumber.EarthshakeMastery, SkillNumber.EarthshakeStreng, SkillNumber.Undefined, 2, 4, SkillNumber.Earthshake, 20, Formula120);
        this.AddMasterSkillDefinition(SkillNumber.CritDmgIncPowUp3, SkillNumber.CritDmgIncPowUp2, SkillNumber.Undefined, 2, 5, SkillNumber.IncreaseCriticalDamage, 20, Formula181);
        this.AddMasterSkillDefinition(SkillNumber.FireScreamStren, SkillNumber.FireScream, SkillNumber.Undefined, 2, 5, SkillNumber.FireScream, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.DarkSpiritStr, Stats.RavenBonusDamage, AggregateType.AddRaw, Formula632, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ScepterStrengthener, Stats.ScepterStrBonusDamage, AggregateType.AddRaw, Formula502, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ShieldStrengthenerLordEmperor, Stats.BonusDefenseWithShield, AggregateType.AddRaw, Formula803, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.UseScepterPetStr, Stats.ScepterPetBonusDamage, AggregateType.AddRaw, Formula632, 2, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.DarkSpiritStr2, Stats.RavenCriticalDamageChance, AggregateType.AddRaw, $"{Formula181} / 100", Formula181, 3, 3, SkillNumber.DarkSpiritStr);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ScepterMastery, Stats.ScepterMasteryBonusDamage, AggregateType.AddRaw, Formula1154, 3, 3, SkillNumber.ScepterStrengthener);
        this.AddPassiveMasterSkillDefinition(SkillNumber.ShieldMastery, Stats.BonusDefenseRateWithShield, AggregateType.AddRaw, Formula1204, 3, 3, SkillNumber.ShieldStrengthenerLordEmperor);
        this.AddPassiveMasterSkillDefinition(SkillNumber.CommandAttackInc, Stats.BonusDamageWithScepterCmdDiv, AggregateType.AddRaw, $"1 / ({Formula3822})", Formula3822, 3, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.DarkSpiritStr3, Stats.RavenExcDamageChance, AggregateType.AddRaw, $"{Formula120} / 100", Formula120, 5, 3, SkillNumber.DarkSpiritStr2);
        this.AddPassiveMasterSkillDefinition(SkillNumber.PetDurabilityStr, Stats.PetDurationIncrease, AggregateType.Multiplicate, Formula1204, 5, 3);

        // RF
        this.AddPassiveMasterSkillDefinition(SkillNumber.DurabilityReduction1FistMaster, Stats.ItemDurationIncrease, AggregateType.Multiplicate, Formula1204, 1, 1);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreasePvPDefenseRate, Stats.DefenseRatePvp, AggregateType.AddRaw, Formula25587, 1, 1);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseMaximumSd, Stats.MaximumShield, AggregateType.AddRaw, Formula30704, 2, 1);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseManaRecoveryRate, Stats.ManaRecoveryMultiplier, AggregateType.AddRaw, FormulaRecoveryIncrease181, Formula181, 2, 1, SkillNumber.Undefined, SkillNumber.Undefined, 20);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreasePoisonResistance, Stats.PoisonResistance, AggregateType.AddRaw, Formula120Value, Formula120, 2, 1);
        this.AddPassiveMasterSkillDefinition(SkillNumber.DurabilityReduction2FistMaster, Stats.ItemDurationIncrease, AggregateType.Multiplicate, Formula1204, 3, 1, SkillNumber.DurabilityReduction1FistMaster);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseSdRecoveryRate, Stats.ShieldRecoveryMultiplier, AggregateType.AddRaw, FormulaRecoveryIncrease120, Formula120, 3, 1, SkillNumber.IncreaseMaximumSd, SkillNumber.Undefined, 20);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseHpRecoveryRate, Stats.HealthRecoveryMultiplier, AggregateType.AddRaw, FormulaRecoveryIncrease120, Formula120, 3, 1, SkillNumber.IncreaseManaRecoveryRate, SkillNumber.Undefined, 20);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseLightningResistance, Stats.LightningResistance, AggregateType.AddRaw, Formula120Value, Formula120, 3, 1, requiredSkill1: SkillNumber.IncreasePoisonResistance);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreasesDefense, Stats.DefenseBase, AggregateType.AddFinal, Formula3371, 4, 1);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreasesAgRecoveryRate, Stats.AbilityRecoveryMultiplier, AggregateType.AddRaw, FormulaRecoveryIncrease120, Formula120, 4, 1, SkillNumber.IncreaseHpRecoveryRate, SkillNumber.Undefined, 20);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseIceResistance, Stats.IceResistance, AggregateType.AddRaw, Formula120Value, Formula120, 4, 1, requiredSkill1: SkillNumber.IncreaseLightningResistance);
        this.AddPassiveMasterSkillDefinition(SkillNumber.DurabilityReduction3FistMaster, Stats.ItemDurationIncrease, AggregateType.Multiplicate, Formula1204, 5, 1, SkillNumber.DurabilityReduction2FistMaster);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseDefenseSuccessRate, Stats.DefenseRatePvm, AggregateType.Multiplicate, FormulaIncreaseMultiplicator120, Formula120, 5, 1, SkillNumber.IncreasesDefense, SkillNumber.Undefined, 20);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseAttackSuccessRate, Stats.AttackRatePvm, AggregateType.AddRaw, Formula20469, 1, 2);
        this.AddMasterSkillDefinition(SkillNumber.KillingBlowStrengthener, SkillNumber.Undefined, SkillNumber.Undefined, 2, 2, SkillNumber.KillingBlow, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.BeastUppercutStrengthener, SkillNumber.Undefined, SkillNumber.Undefined, 2, 2, SkillNumber.BeastUppercut, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.KillingBlowMastery, SkillNumber.KillingBlowStrengthener, SkillNumber.Undefined, 2, 3, SkillNumber.KillingBlow, 20, Formula120);
        this.AddMasterSkillDefinition(SkillNumber.BeastUppercutMastery, SkillNumber.BeastUppercutStrengthener, SkillNumber.Undefined, 2, 3, SkillNumber.BeastUppercut, 20, Formula120);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseMaximumHp, Stats.MaximumHealth, AggregateType.AddRaw, Formula5418, 4, 2);
        this.AddPassiveMasterSkillDefinition(SkillNumber.WeaponMasteryFistMaster, Stats.MasterSkillPhysBonusDmg, AggregateType.AddRaw, Formula502, 4, 2);
        this.AddMasterSkillDefinition(SkillNumber.ChainDriveStrengthener, SkillNumber.ChainDrive, SkillNumber.Undefined, 2, 5, SkillNumber.ChainDrive, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.DarkSideStrengthener, SkillNumber.DarkSide, SkillNumber.Undefined, 2, 5, SkillNumber.DarkSide, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseMaximumMana, Stats.MaximumMana, AggregateType.AddRaw, Formula5418, 5, 2, SkillNumber.IncreaseMaximumHp);
        this.AddMasterSkillDefinition(SkillNumber.DragonRoarStrengthener, SkillNumber.DragonRoar, SkillNumber.Undefined, 2, 5, SkillNumber.DragonRoar, 20, Formula502);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreasePvPAttackRate, Stats.AttackRatePvp, AggregateType.AddRaw, Formula32751, 1, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.EquippedWeaponStrengthener, Stats.GloveWeaponBonusDamage, AggregateType.AddRaw, Formula502, 2, 3);
        this.AddMasterSkillDefinition(SkillNumber.DefSuccessRateIncPowUp, SkillNumber.IncreaseBlock, SkillNumber.Undefined, 3, 2, SkillNumber.IncreaseBlock, 20, Formula502);

        // todo: Increases the probability of Double Damage while using gloves according to the assigned Skill Level.
        this.AddMasterSkillDefinition(SkillNumber.EquippedWeaponMastery, SkillNumber.EquippedWeaponStrengthener, SkillNumber.Undefined, 3, 3, SkillNumber.Undefined, 20, Formula120);
        this.AddMasterSkillDefinition(SkillNumber.DefSuccessRateIncMastery, SkillNumber.DefSuccessRateIncPowUp, SkillNumber.Undefined, 3, 3, SkillNumber.IncreaseBlock, 20, Formula502);
        this.AddMasterSkillDefinition(SkillNumber.StaminaIncreaseStrengthener, SkillNumber.IncreaseHealth, SkillNumber.Undefined, 3, 4, SkillNumber.IncreaseHealth, 20, Formula1154);
        this.AddMasterSkillDefinition(SkillNumber.DecreaseMana, SkillNumber.Undefined, SkillNumber.Undefined, 3, 4, SkillNumber.Undefined, 20, Formula722);
        this.AddPassiveMasterSkillDefinition(SkillNumber.RecoverSDfromMonsterKills, Stats.ShieldAfterMonsterKillMultiplier, AggregateType.AddFinal, Formula914, 4, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.RecoverHPfromMonsterKills, Stats.HealthAfterMonsterKillMultiplier, AggregateType.AddFinal, Formula4319, 4, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.IncreaseMinimumAttackPower, Stats.MinimumPhysBaseDmg, AggregateType.AddRaw, Formula502, 5, 3);
        this.AddPassiveMasterSkillDefinition(SkillNumber.RecoverManaMonsterKills, Stats.ManaAfterMonsterKillMultiplier, AggregateType.AddFinal, Formula4319, 5, 3, SkillNumber.RecoverHPfromMonsterKills);
        */
    }

    private void AddPassiveMasterSkillDefinition(int skillNumber, AttributeDefinition targetAttribute, AggregateType aggregateType, string valueFormula, string displayValueFormula, byte rank, byte root, int requiredSkill1 = 0, int requiredSkill2 = 0, byte maximumLevel = 20)
    {
        this.AddMasterSkillDefinition(skillNumber, requiredSkill1, requiredSkill2, root, rank, 0, maximumLevel, valueFormula, displayValueFormula, targetAttribute, aggregateType);
    }

    private void AddPassiveMasterSkillDefinition(int skillNumber, AttributeDefinition targetAttribute, AggregateType aggregateType, string valueFormula, byte rank, byte root, int requiredSkill1 = 0, int requiredSkill2 = 0, byte maximumLevel = 20)
    {
        this.AddMasterSkillDefinition(skillNumber, requiredSkill1, requiredSkill2, root, rank, 0, maximumLevel, valueFormula, valueFormula, targetAttribute, aggregateType);
    }

    private void AddMasterSkillDefinition(int skillNumber, int requiredSkill1, int requiredSkill2, byte root, byte rank, int regularSkill, byte maximumLevel, string valueFormula, bool extendsDuration = false)
    {
        this.AddMasterSkillDefinition(skillNumber, requiredSkill1, requiredSkill2, root, rank, regularSkill, maximumLevel, valueFormula, valueFormula, null, AggregateType.AddRaw, extendsDuration);
    }

    private void AddMasterSkillDefinition(int skillNumber, int requiredSkill1, int requiredSkill2, byte root, byte rank, int regularSkill, byte maximumLevel, string valueFormula, string displayValueFormula, AttributeDefinition? targetAttribute, AggregateType aggregateType, bool extendsDuration = false)
    {
        var skill = this.GameConfiguration.Skills.First(s => s.Number == (short)skillNumber);
        skill.MasterDefinition = this.Context.CreateNew<MasterSkillDefinition>();
        skill.MasterDefinition.Rank = rank;
        skill.MasterDefinition.Root = this._masterSkillRoots[root];
        skill.MasterDefinition.ValueFormula = valueFormula;
        skill.MasterDefinition.DisplayValueFormula = displayValueFormula;
        skill.MasterDefinition.MaximumLevel = maximumLevel;
        skill.MasterDefinition.TargetAttribute = targetAttribute?.GetPersistent(this.GameConfiguration);
        skill.MasterDefinition.Aggregation = aggregateType;
        skill.MasterDefinition.ReplacedSkill = this.GameConfiguration.Skills.FirstOrDefault(s => s.Number == (short)regularSkill);
        skill.MasterDefinition.ExtendsDuration = extendsDuration;
        if (requiredSkill1 != 0)
        {
            skill.MasterDefinition.RequiredMasterSkills.Add(this.GameConfiguration.Skills.First(s => s.Number == (short)requiredSkill1));
        }

        if (requiredSkill2 != 0)
        {
            skill.MasterDefinition.RequiredMasterSkills.Add(this.GameConfiguration.Skills.First(s => s.Number == (short)requiredSkill2));
        }

        if (maximumLevel == 10 && valueFormula == Formula1WhenComplete)
        {
            skill.MasterDefinition.MinimumLevel = maximumLevel;
        }
        else
        {
            skill.MasterDefinition.MinimumLevel = 1;
        }

        var replacedSkill = skill.MasterDefinition.ReplacedSkill;
        if (replacedSkill != null)
        {
            // Because we don't want to duplicate code from the replaced skills to the master skills, we just assign some values from the replaced skill.
            // These describe the skill behavior.
            skill.AttackDamage = replacedSkill.AttackDamage;
            skill.DamageType = replacedSkill.DamageType;
            skill.ElementalModifierTarget = replacedSkill.ElementalModifierTarget;
            skill.ImplicitTargetRange = replacedSkill.ImplicitTargetRange;
            skill.MovesTarget = replacedSkill.MovesTarget;
            skill.MovesToTarget = replacedSkill.MovesToTarget;
            skill.SkillType = replacedSkill.SkillType;
            skill.Target = replacedSkill.Target;
            skill.TargetRestriction = replacedSkill.TargetRestriction;
            skill.MagicEffectDef = replacedSkill.MagicEffectDef;

            if (replacedSkill.AreaSkillSettings is { } areaSkillSettings)
            {
                skill.AreaSkillSettings = this.Context.CreateNew<AreaSkillSettings>();
                var id = skill.AreaSkillSettings.GetId();
                skill.AreaSkillSettings.AssignValuesOf(areaSkillSettings, this.GameConfiguration);
                skill.AreaSkillSettings.SetGuid(id);
            }
        }
    }

    private void CreateSpecialSummonMonsters()
    {
        {
            var monster = this.Context.CreateNew<MonsterDefinition>();
            this.GameConfiguration.Monsters.Add(monster);
            monster.Number = 150;
            monster.Designation = "Bali";
            monster.MoveRange = 3;
            monster.AttackRange = 1;
            monster.ViewRange = 7;
            monster.MoveDelay = new TimeSpan(400 * TimeSpan.TicksPerMillisecond);
            monster.AttackDelay = new TimeSpan(1600 * TimeSpan.TicksPerMillisecond);
            monster.RespawnDelay = new TimeSpan(100 * TimeSpan.TicksPerSecond);
            monster.Attribute = 2;
            monster.NumberOfMaximumItemDrops = 1;
            var attributes = new Dictionary<AttributeDefinition, float>
            {
                { Stats.Level, 52 },
                { Stats.MaximumHealth, 5000 },
                { Stats.MinimumPhysBaseDmg, 165 },
                { Stats.MaximumPhysBaseDmg, 170 },
                { Stats.DefenseBase, 100 },
                { Stats.AttackRatePvm, 260 },
                { Stats.DefenseRatePvm, 75 },
                { Stats.PoisonResistance, 6f / 255 },
                { Stats.IceResistance, 6f / 255 },
                { Stats.WaterResistance, 6f / 255 },
                { Stats.FireResistance, 6f / 255 },
            };

            monster.AddAttributes(attributes, this.Context, this.GameConfiguration);
            monster.SetGuid(monster.Number);
        }

        {
            var monster = this.Context.CreateNew<MonsterDefinition>();
            this.GameConfiguration.Monsters.Add(monster);
            monster.Number = 151;
            monster.Designation = "Soldier";
            monster.MoveRange = 3;
            monster.AttackRange = 4;
            monster.ViewRange = 7;
            monster.MoveDelay = new TimeSpan(400 * TimeSpan.TicksPerMillisecond);
            monster.AttackDelay = new TimeSpan(1600 * TimeSpan.TicksPerMillisecond);
            monster.RespawnDelay = new TimeSpan(100 * TimeSpan.TicksPerSecond);
            monster.Attribute = 2;
            monster.NumberOfMaximumItemDrops = 1;
            var attributes = new Dictionary<AttributeDefinition, float>
            {
                { Stats.Level, 58 },
                { Stats.MaximumHealth, 4000 },
                { Stats.MinimumPhysBaseDmg, 175 },
                { Stats.MaximumPhysBaseDmg, 180 },
                { Stats.DefenseBase, 110 },
                { Stats.AttackRatePvm, 290 },
                { Stats.DefenseRatePvm, 86 },
                { Stats.PoisonResistance, 6f / 255 },
                { Stats.IceResistance, 6f / 255 },
                { Stats.WaterResistance, 6f / 255 },
                { Stats.FireResistance, 6f / 255 },
            };

            monster.AddAttributes(attributes, this.Context, this.GameConfiguration);
            monster.SetGuid(monster.Number);
        }
    }
}
