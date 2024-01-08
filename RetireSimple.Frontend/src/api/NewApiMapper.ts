import { AnyObject } from 'yup';
import {
	ApiFormData,
	ApiPresetData,
	Investment,
	InvestmentModel,
	InvestmentVehicle,
	InvestmentVehicleModel,
	PortfolioModel,
} from '../Interfaces';
import { API_BASE_URL } from './ApiCommon';

export const convertVehicleModelData = (model: InvestmentVehicleModel) => {
	const result = [];
	

	for (let i = 0; i < model.avgModelData.length; i++) {
		result.push({
			year: i,
			avg: +model.avgModelData[i].toFixed(2),
		});
	}
	return result;
};


export const getTestData = async () : Promise<InvestmentVehicle[]> => {
	const response = await fetch(`${API_BASE_URL}/NewVehicle/Test`);
	return await response.json();
}