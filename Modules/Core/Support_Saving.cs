//Oh boy this is going to be.. Interesting do to. Gotta start somewhere.

$EOTW::SaveLocation = "config/server/SAEX2/";
function EOTW_SaveData()
{
    //Save Player Data
    for (%i = 0; %i < ClientGroup.getCount(); %i++)
        EOTW_SaveData_PlayerData(ClientGroup.getObject(%i));
}

function EOTW_SaveData_PlayerData(%client)
{
    %player = %client.player;
    %saveDir = $EOTW::SaveLocation @ "/PlayerData/" @ %client.bl_id @ "/";
    
    //Save Materials
    export("$EOTW::Material" @ %client.bl_id @ "*", %saveDir @ "Materials.cs");

    //Save Player Battery
    %file = new FileObject();
    if(isObject(%player) && %file.openForWrite(%saveDir @ "Battery.cs"))
    {
        %file.writeLine("BATTERY" TAB %player.GetBatteryEnergy());
        %file.writeLine("MAXBATTERY" TAB %player.GetMaxBatteryEnergy());
    }
    %file.close();
    %file.delete();

    //Save Position & Checkpoint Position
    %file = new FileObject();
    if(isObject(%player) && %file.openForWrite(%saveDir @ "Position.cs"))
    {
        %file.writeLine("POSITION" TAB %player.getTransform());
        if (isObject(%client.checkpointBrick))
            %file.writeLine("CHECKPOINT" TAB %client.checkpointBrick.getPosition());
    }
    %file.close();
    %file.delete();
}

function EOTW_LoadData_PlayerData(%client)
{
    %player = %client.player;
    %saveDir = $EOTW::SaveLocation @ "/PlayerData/" @ %client.bl_id @ "/";

    //Load Materials
    %file = new FileObject();
    %file.openForRead(%saveDir @ "Materials.cs");
    while(!%file.isEOF())
    {
        %line = %file.readLine();
        
        //HERESY, HERESY, I DIDN'T HAVE TO TAKE THIS PATH BUT YET I DID. I COULD OF JUST USED SOME FUNCTION TO SHORTEN THIS.
        %subLen = strLen(getSubStr(%line, 0, strPos(%line, "=") - 1)) - strLen(getSubStr(%line, 0, strPos(%line, "_") + 1));
        %eval = getSubStr(%line, 0, strPos(%line, "_") + 1) @ "[\"" @ getSubStr(%line, strPos(%line, "_") + 1, %subLen) @ "\"] " @ getSubStr(%line, strPos(%line, "="), 420);
        eval(%eval);
    }
    %file.close();
    %file.delete();

    //Load Battery
    %file = new FileObject();
    %file.openForRead(%saveDir @ "Battery.cs");
    while(!%file.isEOF() && isObject(%player))
    {
        %line = %file.readLine();
        switch$ (getField(%line, 0))
        {
            case "BATTERY":
                %player.SetBatteryEnergy(getField(%line, 1));
            case "MAXBATTERY":
                %player.SetMaxBatteryEnergy(getField(%line, 1));
        }
    }
    %file.close();
    %file.delete();

    //Load Position
    %file = new FileObject();
    %file.openForRead(%saveDir @ "Position.cs");
    while(!%file.isEOF() && isObject(%player))
    {
        %line = %file.readLine();
        switch$ (getField(%line, 0))
        {
            case "POSITION":
                %player.setTransform(getField(%line, 1));
            case "CHECKPOINT":
                initContainerRadiusSearch(getField(%line, 1), 0.1, $TypeMasks::fxBrickAlwaysObjectType);
                while(isObject(%hit = containerSearchNext()))
                {
                    if(%hit.getDataBlock().getName() $= "brickCheckpointData")
                    {
                        %client.checkpointBrick = %hit;
                        break;
                    }
                }
        }
    }
    %file.close();
    %file.delete();
}