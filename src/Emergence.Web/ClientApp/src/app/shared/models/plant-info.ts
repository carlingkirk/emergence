import { User } from "oidc-client";
import { LightType, Month, StratificationType, Unit, Visibility, WaterType } from "./enums";
import { Lifeform } from "./lifeform";
import { Origin } from "./origin";
import { Photo } from "./photo";

export class PlantInfo {
    plantInfoId: number;
    lifeformId: number;
    bloomTime: BloomTime;
    height: Height;
    spread: Spread;
    requirements : Requirements;
    visibility: Visibility;
    name: string;
    notes: string;
    userId: number;
    createdBy: string;
    dateCreated: Date;
    modifiedBy: string;
    dateModified: Date;
    origin: Origin;
    lifeform: Lifeform;
    photos: Photo[];
    user: User;
  }
  
  export class BloomTime {
    minimumBloomTime: Month;
    maximumBloomTime: Month;
  }

  export class Height {
    minimumHeight: number;
    maximumHeight: number;
    unit: Unit;
  }

  export class Spread {
    minimumSpread: number;
    maximumSpread: number;
    unit: Unit;
  }

  export class Requirements {
    waterRequirements: WaterRequirements;
    lightRequirements: LightRequirements;
    zoneRequirements: ZoneRequirements;
    stratificationStages: StratificationStage[];
  }

  export class WaterRequirements {
    minimumWater: WaterType;
    maximumWater: WaterType;
  }
  
  export class LightRequirements {
    minimumLight: LightType;
    maximumLight: LightType;
  }

  export class ZoneRequirements {
    minimumZone: Zone;
    maximumZone: Zone;
  }

  export class Zone {
      id: number;
      name: string;
  }

  export class StratificationStage {
      step: number;
      dayLength: number;
      stratificationType: StratificationType;
  }