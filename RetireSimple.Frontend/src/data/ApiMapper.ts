import {Investment, InvestmentModel, PortfolioModel} from './Interfaces';

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

// export const createAggregateStackData = (investmentModels: {[key: string]: InvestmentModel}) => {
// 	const result = [];

// 	for (let i = 0; i < ; i++) {
// 		result.push({
// 			year: i,
// 			[]: +model.avgModelData[i].toFixed(2),
// 			min: +model.minModelData[i].toFixed(2),
// 			max: +model.maxModelData[i].toFixed(2),
// 		});
// 	}

// 	return result;
// };
