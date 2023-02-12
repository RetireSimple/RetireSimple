import {yupResolver} from '@hookform/resolvers/yup';
import {Box, Button, Divider} from '@mui/material';
import React from 'react';
import {FormProvider, useForm} from 'react-hook-form';
import {useFormAction, useLoaderData, useSubmit} from 'react-router-dom';
import {InvestmentModelGraph} from '../components/InvestmentModelGraph';
import {InvestmentFormDefaults, investmentFormSchema} from '../data/FormSchema';
import {Investment} from '../data/Interfaces';
import {InvestmentDataForm} from '../forms/InvestmentDataForm';

export const InvestmentView = () => {
	const currentInvestmentData = useLoaderData() as Investment;
	const submit = useSubmit();
	const deleteAction = useFormAction('delete');

	const investmentDataFormContext = useForm({
		shouldUnregister: true,
		resolver: yupResolver(investmentFormSchema),
		defaultValues: InvestmentFormDefaults,
	});
	const {reset} = investmentDataFormContext;

	//HACK React docs indicate this is problematic, should fix sometime
	React.useEffect(() => {
		reset(currentInvestmentData);
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [currentInvestmentData]);

	const handleDelete = () => {
		submit(null, {action: deleteAction, method: 'delete'});
	};

	return (
		<>
			<FormProvider {...investmentDataFormContext}>
				<InvestmentDataForm
					defaultValues={currentInvestmentData}
					disableTypeSelect={true} />
				<Divider />
				<Button variant='contained' onClick={() => reset(currentInvestmentData)}>Reset</Button>
				<Button variant='contained' onClick={handleDelete}>Delete</Button>
			</FormProvider>

			<Box sx={{width: '100%', height: '100%'}}>
				<InvestmentModelGraph investmentId={currentInvestmentData.investmentId} />
			</Box>
		</>
	);
};
