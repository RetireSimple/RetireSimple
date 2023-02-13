import {yupResolver} from '@hookform/resolvers/yup';
import {Box, Button, Divider} from '@mui/material';
import React from 'react';
import {FormProvider, useForm, useFormState} from 'react-hook-form';
import {FieldValues} from 'react-hook-form/dist/types';
import {useFormAction, useLoaderData, useSubmit} from 'react-router-dom';
import {updateInvestment} from '../api/InvestmentApi';
import {InvestmentModelGraph} from '../components/InvestmentModelGraph';
import {InvestmentFormDefaults, investmentFormSchema} from '../data/FormSchema';
import {Investment} from '../data/Interfaces';
import {InvestmentDataForm} from '../forms/InvestmentDataForm';

export const InvestmentView = () => {
	const currentInvestmentData = useLoaderData() as Investment;
	const submit = useSubmit();
	const deleteAction = useFormAction('delete');
	const updateAction = useFormAction('update');
	const formContext = useForm({
		shouldUnregister: true,
		resolver: yupResolver(investmentFormSchema),
		defaultValues: InvestmentFormDefaults,
	});

	const {reset, control, handleSubmit} = formContext;
	const {isDirty, dirtyFields} = useFormState({control});


	//HACK React docs indicate this is problematic, should fix sometime
	React.useEffect(() => {
		formContext.reset(currentInvestmentData, {keepErrors: true});
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [currentInvestmentData]);

	const handleDelete = () => {
		submit(null, {action: deleteAction, method: 'delete'});
	};

	const handleUpdate =
		handleSubmit((data: FieldValues) => {
			console.log('submitting')
			const requestData: {[key: string]: string;} = {};

			Object.entries(dirtyFields).forEach(([key, value])=> {
				if(value === true) {
					requestData[key] = data[key].toString()
				}
			});

			updateInvestment(currentInvestmentData.investmentId, requestData).then(() => {
				submit(null, {action: updateAction, method: 'post'});
			});
		},
		);


	return (
		<Box sx={{display: 'flex', flexDirection:'column'}}>
			<Box sx={{display: 'flex', flexDirection: 'column', justifyContent: 'flex-start'}}>
				<FormProvider {...formContext}>
					<InvestmentDataForm
						defaultValues={currentInvestmentData}
						disableTypeSelect={true}>
						<Divider sx={{paddingY:'5px'}} />
						<Box sx={{display: 'flex', flexDirection: 'row', justifyContent: 'flex-end' }}>
							<Button onClick={() => reset(currentInvestmentData)}>Reset</Button>
							<Button color='error' onClick={handleDelete}>Delete</Button>
							<Button onClick={handleUpdate} disabled={!isDirty}>Update</Button>
						</Box>
					</InvestmentDataForm>
				</FormProvider>
			</Box>
			<Box sx={{width: '100%', height: '100%'}}>
				<InvestmentModelGraph investmentId={currentInvestmentData.investmentId} />
			</Box>
		</Box>
	);
};
