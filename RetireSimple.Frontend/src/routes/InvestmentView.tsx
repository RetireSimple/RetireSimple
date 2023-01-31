import React from 'react';
import {FieldValues, FormProvider, useForm} from 'react-hook-form';
import {InvestmentDataForm} from '../forms/InvestmentDataForm';

export const InvestmentView = () => {

	// const renderAnalysis = () => {
	// 	return (<InvestmentModelGraph
	// 		modelData={investmentModels}
	// 		dataLength={investmentModels?.avgModelData.length} />);
	// };


	const investmentDataFormContext = useForm();


	// let chart = renderAnalysis();

	return (
		<>
			<FormProvider {...investmentDataFormContext}>
				<InvestmentDataForm onSubmit={(data: FieldValues) => {console.log(data);}} />
			</FormProvider>
			{/*
					<input type="text" onChange={e => setInvestmentModelId(e.target.value)}></input>
					<button onClick={getAnalysis}>Get Analysis Model</button>

					{chart} */}
		</>
	);
};
