import {yupResolver} from '@hookform/resolvers/yup';
import {Box, Button, Divider, Tab, Tabs, Typography} from '@mui/material';
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
import {convertDates} from '../api/ConvertUtils';
import {ExpensesTable} from '../forms/ExpenseTable';
import {useSnackbar} from 'notistack';

interface InvestmentViewTabProps {
	tab: number;
	children?: React.ReactNode;
	value: number;
}

const InvestmentViewTab = (props: InvestmentViewTabProps) => {
	return (
		<Box sx={{display: 'flex', flexDirection: 'column', justifyContent: 'flex-start'}}>
			{props.tab === props.value && <Box sx={{p: 1}}>{props.children}</Box>}
		</Box>
	);
};

export const InvestmentView = () => {
	const [showDelete, setShowDelete] = React.useState(false);
	const [tab, setTab] = React.useState(0);

	const currentInvestmentData = useLoaderData() as Investment;
	const submit = useSubmit();
	const deleteAction = useFormAction('delete');
	const updateAction = useFormAction('update');
	const formContext = useForm({
		shouldUnregister: true,
		//resolver: yupResolver(investmentFormSchema),
		defaultValues: currentInvestmentData ?? InvestmentFormDefaults,
	});
	const {enqueueSnackbar} = useSnackbar();

	const {reset, control, handleSubmit} = formContext;
	const {isDirty, dirtyFields} = useFormState({control});

	React.useEffect(() => {
		reset(currentInvestmentData, {keepErrors: true});
	}, [currentInvestmentData, reset]);

	const handleUpdate = handleSubmit((data: FieldValues) => {
		const requestData: {[key: string]: string} = {};
		Object.entries(dirtyFields).forEach(([key, value]) => {
			if (value === true) {
				requestData[key] = data[key].toString();
			}
		});

		convertDates(requestData);

		updateInvestment(currentInvestmentData.investmentId, requestData)
			.then(() => {
				enqueueSnackbar('Investment updated successfully.', {variant: 'success'});
				submit(null, {action: updateAction, method: 'post'});
			})
			.catch((error) => {
				enqueueSnackbar(`Failed to update investment: ${error.message}`, {
					variant: 'error',
				});
			});
	});

	return (
		<Box sx={{display: 'flex', flexDirection: 'column', minWidth: '60rem'}}>
			<Tabs value={tab} onChange={(e, v) => setTab(v)}>
				<Tab label='Investment Details' />
				<Tab label='Expense Information' />
			</Tabs>
			<InvestmentViewTab value={tab} tab={0}>
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
			</InvestmentViewTab>
			<InvestmentViewTab value={tab} tab={1}>
				<Typography variant='h6' component='div' sx={{flexGrow: 1, marginBottom: '1rem'}}>
					Expense Information:
					<ExpensesTable investmentId={currentInvestmentData.investmentId} />
				</Typography>
			</InvestmentViewTab>
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
