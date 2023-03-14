import {
	FormVehicle,
	Investment,
	InvestmentModel,
	InvestmentVehicle,
	PortfolioModel,
} from '../Interfaces';

//Flattens API Investment object for form use
//todo: should we strip fields items?
export const flattenApiInvestment = (investment: Investment) => {
	const result: {[key: string]: any} = {};

	result['investmentId'] = investment.investmentId;
	result['investmentName'] = investment.investmentName;
	result['investmentType'] = investment.investmentType;
	result['analysisType'] = investment.analysisType;

	Object.assign(result, investment.investmentData);

	const renamedOverrides = Object.keys(investment.analysisOptionsOverrides).reduce((acc, key) => {
		acc[`analysis_${key}`] = investment.analysisOptionsOverrides[key];
		return acc;
	}, {} as {[key: string]: any});

	Object.assign(result, renamedOverrides);

	return result;
};

//Converts InvestmentModel Data for use with Recharts
export const convertInvestmentModelData = (model: InvestmentModel) => {
	const result = [];

	for (let i = 0; i < model.avgModelData.length; i++) {
		result.push({
			year: i,
			avg: +model.avgModelData[i].toFixed(2),
			min: +model.minModelData[i].toFixed(2),
			max: +model.maxModelData[i].toFixed(2),
		});
	}

	return result;
};

export const convertPortfolioModelData = (model: PortfolioModel) => {
	const result = [];

	for (let i = 0; i < model.avgModelData.length; i++) {
		result.push({
			year: i,
			avg: +model.avgModelData[i].toFixed(2),
			min: +model.minModelData[i].toFixed(2),
			max: +model.maxModelData[i].toFixed(2),
		});
	}

	return result;
};

export const createAggregateStackData = (investmentModels: {[key: string]: InvestmentModel}) => {
	const result = [];

	const exModel = Object.values(investmentModels)[0];

	for (let i = 0; i < exModel.avgModelData.length; i++) {
		const dataPoint: {[key: string]: number} = {month: i};

		Object.keys(investmentModels).forEach((key) => {
			const model = investmentModels[key];
			dataPoint[key] = +model.avgModelData[i].toFixed(2);
		});

		result.push(dataPoint);
	}

	return result;
};

export const getFlatVehicleData = (vehicle: InvestmentVehicle): FormVehicle => {
	const result: FormVehicle = {
		investmentVehicleId: vehicle.investmentVehicleId,
		investmentVehicleName: vehicle.investmentVehicleName,
		investmentVehicleType: vehicle.investmentVehicleType,
		cashHoldings: vehicle.investmentVehicleData['cashHoldings'],
		analysis_analysisLength: vehicle.analysisOptionsOverrides['analysisLength'],
		analysis_CashContribution: vehicle.analysisOptionsOverrides['cashContribution'],
		analysis_vehicleTaxPercentage: vehicle.analysisOptionsOverrides['vehicleTaxPercentage'],
	};

	return result;
};
