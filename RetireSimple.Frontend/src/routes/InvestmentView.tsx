import {yupResolver} from '@hookform/resolvers/yup';
import {Box, Divider} from '@mui/material';
import React from 'react';
import {FormProvider, useForm} from 'react-hook-form';
import {Outlet, useLoaderData} from 'react-router-dom';
import {InvestmentModelGraph} from '../components/InvestmentModelGraph';
import {InvestmentFormDefaults, investmentFormSchema} from '../data/FormSchema';
import {Investment} from '../data/Interfaces';
import {InvestmentDataForm} from '../forms/InvestmentDataForm';

export const InvestmentView = () => {

	const currentInvestmentData = useLoaderData() as Investment

	const investmentDataFormContext = useForm({
		shouldUnregister:true,
		resolver: yupResolver(investmentFormSchema),
		defaultValues: InvestmentFormDefaults});
	const {reset} = investmentDataFormContext;

	React.useEffect(() => {
		reset(currentInvestmentData);
	// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [currentInvestmentData]);

	return (
		<>
			<FormProvider {...investmentDataFormContext}>
				<InvestmentDataForm defaultValues={currentInvestmentData} />
				<Divider />
				<Box sx={{width: '100%', height: '100%'}}>
					<InvestmentModelGraph investmentId={currentInvestmentData.investmentId} />
				</Box>
				{/* {graph} */}
			</FormProvider>

			<Outlet />
		</>
	);
};
