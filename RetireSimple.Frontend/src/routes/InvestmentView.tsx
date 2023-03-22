import {yupResolver} from '@hookform/resolvers/yup';
import {Box, Button, Divider, Typography} from '@mui/material';
import React from 'react';
import {FormProvider, useForm, useFormState} from 'react-hook-form';
import {FieldValues} from 'react-hook-form/dist/types';
import {useFormAction, useLoaderData, useSubmit} from 'react-router-dom';
import {Investment} from '../Interfaces';
import {updateInvestment} from '../api/InvestmentApi';
import {ConfirmDeleteDialog} from '../components/DialogComponents';
import {InvestmentModelGraph} from '../components/GraphComponents';
import {InvestmentFormDefaults, investmentFormSchema} from '../forms/FormSchema';
import {InvestmentDataForm} from '../forms/InvestmentDataForm';

export const InvestmentView = () => {
	const [showDelete, setShowDelete] = React.useState(false);

	const currentInvestmentData = useLoaderData() as Investment;
	const submit = useSubmit();
	const deleteAction = useFormAction('delete');
	const updateAction = useFormAction('update');
	const formContext = useForm({
		shouldUnregister: true,
		resolver: yupResolver(investmentFormSchema),
		defaultValues: currentInvestmentData ?? InvestmentFormDefaults,
	});

	const {reset, control, handleSubmit} = formContext;
	const {isDirty, dirtyFields} = useFormState({control});

	React.useEffect(() => {
		console.log('currentInvestmentData is defined, resetting form');
		reset(currentInvestmentData, {keepErrors: true});
	}, [currentInvestmentData, reset]);

	const handleUpdate = handleSubmit((data: FieldValues) => {
		const requestData: {[key: string]: string} = {};
		Object.entries(dirtyFields).forEach(([key, value]) => {
			if (value === true) {
				requestData[key] = data[key].toString();
			}
		});
		updateInvestment(currentInvestmentData.investmentId, requestData).then(() => {
			submit(null, {action: updateAction, method: 'post'});
		});
	});

	return (
		<Box sx={{display: 'flex', flexDirection: 'column'}}>
			<Box sx={{display: 'flex', flexDirection: 'column', justifyContent: 'flex-start'}}>
				<Typography variant='h6' component='div' sx={{flexGrow: 1, marginBottom: '1rem'}}>
					Investment Details: {currentInvestmentData.investmentName}
				</Typography>
				<FormProvider {...formContext}>
					<InvestmentDataForm
						defaultValues={currentInvestmentData}
						disableTypeSelect={true}>
						<Divider sx={{paddingY: '5px'}} />
						<Box
							sx={{
								display: 'flex',
								flexDirection: 'row',
								justifyContent: 'flex-end',
							}}>
							<Button onClick={() => reset(currentInvestmentData)}>Reset</Button>
							<Button color='error' onClick={() => setShowDelete(true)}>
								Delete
							</Button>
							<Button onClick={handleUpdate} disabled={!isDirty}>
								Update
							</Button>
						</Box>
					</InvestmentDataForm>
				</FormProvider>
			</Box>
			<Box sx={{width: '100%', height: '100%'}}>
				<InvestmentModelGraph investmentId={currentInvestmentData.investmentId} />
			</Box>
			<ConfirmDeleteDialog
				open={showDelete}
				onClose={() => setShowDelete(false)}
				onConfirm={() => submit(null, {action: deleteAction, method: 'delete'})}
				deleteTarget={currentInvestmentData.investmentName}
				deleteTargetType='investment'
			/>
		</Box>
	);
};
