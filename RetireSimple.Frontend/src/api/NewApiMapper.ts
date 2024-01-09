import { AnyObject } from 'yup';
import {
	ApiFormData,
	ApiPresetData,
	Investment,
	InvestmentModel,
	InvestmentVehicle,
	InvestmentVehicleModel,
	PortfolioModel,
	Projection,
} from '../Interfaces';
import { API_BASE_URL } from './ApiCommon';

export const convertVehicleModelData = (model: Projection) => {
	const result = [];
	

	for (let i = 0; i < model.values.length; i++) {
		result.push({
			year: i,
			avg: +model.values[i].toFixed(2),
		});
	}
	return result;
};


export const getTestData = async () : Promise<Projection> => {
	const response = await fetch(`${API_BASE_URL}/NewVehicle/Test`);
	console.log("HERE");
	console.log(response.body);
	return await response.json();
}