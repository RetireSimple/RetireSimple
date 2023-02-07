import {Investment} from './Interfaces';

//Flattens API Investment object for form use
//todo: should we strip fields items?
export const flattenApiInvestment = (investment: Investment) => {
	const result: {[key: string]: any} = {};

	result['investmentId'] = investment.investmentId;
	result['investmentName'] = investment.investmentName;
	result['investmentType'] = investment.investmentType;
	result['analysisType'] = investment.analysisType;

	Object.assign(result, investment.investmentData);
	Object.assign(result, investment.analysisOptionsOverrides);

	return result;
};
