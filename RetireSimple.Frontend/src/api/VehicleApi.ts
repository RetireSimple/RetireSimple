import {InvestmentVehicle} from '../Interfaces';
import {API_BASE_URL} from './ApiCommon';

export const getVehicles = async (): Promise<InvestmentVehicle[]> => {
	const response = await fetch(`${API_BASE_URL}/Vehicle`);
	return await response.json();
};

export const getVehicle = async (id: number): Promise<InvestmentVehicle> => {
	const response = await fetch(`${API_BASE_URL}/Vehicle/${id}`);
	return await response.json();
};

export const addVehicle = async (data: any) => {
	await fetch(`${API_BASE_URL}/Vehicle`, {
		method: 'POST',
		body: JSON.stringify(data),
		headers: {
			Accept: 'application/json',
			'Content-Type': 'application/json; charset=utf-8',
		},
	});
};

export const deleteVehicle = async (id: number) => {
	await fetch(`${API_BASE_URL}/Vehicle/${id}`, {
		method: 'DELETE',
	});
};

export const updateVehicle = async (id: number, data: any) => {
	await fetch(`${API_BASE_URL}/Vehicle/${id}`, {
		method: 'POST',
		body: JSON.stringify(data),
		headers: {
			Accept: 'application/json',
			'Content-Type': 'application/json; charset=utf-8',
		},
	});
};

export const addInvestmentToVehicle = async (vehicleId: number, investmentId: number) => {
	await fetch(`${API_BASE_URL}/Vehicle/InvestmentAdd/${vehicleId}?investmentId=${investmentId}`, {
		method: 'POST',
	});
};
