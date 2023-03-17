export interface Investment {
	investmentId: number;
	investmentName: string;
	investmentType: string;
	analysisType?: string;
	lastAnalysis?: string | null; //treat as date later?
	portfolioId: number;
	investmentData: {[key: string]: string};
	analysisOptionsOverrides: {[key: string]: string};
}

export interface InvestmentModel {
	investmentModelId: number;
	investmentId: number;
	maxModelData: number[];
	minModelData: number[];
	avgModelData: number[];
	lastUpdated: string; //treat as a date later?
}

export interface PortfolioModel {
	portfolioModelId: number;
	portfolioId: number;
	maxModelData: number[];
	minModelData: number[];
	avgModelData: number[];
	lastUpdated: string; //treat as a date later?
}

export interface FullModelData {
	portfolioModel: PortfolioModel;
	investmentModels: {[key: string]: InvestmentModel};
}

export interface InvestmentVehicle {
	portfolioId: number;
	investmentVehicleId: number;
	investmentVehicleName: string;
	investmentVehicleType: string;
	investments: Investment[];
	investmentVehicleModelId?: number;
	investmentVehicleData: {[key: string]: string};
	lastUpdated: string; //treat as a date later?
	analysisOptionsOverrides: {[key: string]: string};
}

export interface FormVehicle {
	investmentVehicleId: number;
	investmentVehicleName: string;
	investmentVehicleType: string;
	cashHoldings: string;
	analysis_analysisLength: string;
	analysis_cashContribution: string;
	analysis_vehicleTaxPercentage: string;
}

export interface Portfolio {
	portfolioId: number;
	portfolioName: string;
	profileId: number;
	investments: Investment[];
	investmentVehicles: InvestmentVehicle[];
	portfolioModel: PortfolioModel;
	lastUpdated: string; //treat as a date later?
}

export interface InvestmentVehicleModel {
	modelId: number;
	investmentVehicleId: number;
	lastUpdated: string;
	maxModelData: number[];
	minModelData: number[];
	avgModelData: number[];
	taxDeductedMaxModelData: number[];
	taxDeductedMinModelData: number[];
	taxDeductedAvgModelData: number[];
}