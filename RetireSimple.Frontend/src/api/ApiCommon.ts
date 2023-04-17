import {ApiPresetData, Portfolio} from '../Interfaces';
import {convertApiPresets} from './ApiMapper';

export const API_BASE_URL = import.meta.env.VITE_API_URL;

export const getPortfolio = async (): Promise<Portfolio> => {
	const response = await fetch(`${API_BASE_URL}/Portfolio`);
	const data = await response.json();
	return data;
};

export const getAnalysisPresets = async (): Promise<ApiPresetData> => {
	const response = await fetch(`${API_BASE_URL}/Analysis/Presets`);
	const data = await response.json().then((data) => convertApiPresets(data));
	return data;
};
