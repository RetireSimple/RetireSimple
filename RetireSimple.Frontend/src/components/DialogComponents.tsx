import {yupResolver} from '@hookform/resolvers/yup';
import {Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, Grid} from '@mui/material';
import {FieldValues, FormProvider, useForm, useWatch} from 'react-hook-form';
import {useFormAction, useSubmit} from 'react-router-dom';
import {convertDates} from '../api/ConvertUtils';
import {addExpense, addInvestment} from '../api/InvestmentApi';
import {addInvestmentToVehicle, addVehicle} from '../api/VehicleApi';
import {investmentFormSchema, vehicleFormSchema} from '../forms/FormSchema';
import {InvestmentDataForm} from '../forms/InvestmentDataForm';
import {VehicleDataForm} from '../forms/VehicleDataForm';
import {
	FormTextFieldCurrency,
	FormSelectField,
	FormDatePicker,
	FormTextField,
} from './InputComponents';
import {enqueueSnackbar, useSnackbar} from 'notistack';
import { Investment } from '../Interfaces';

export interface AddInvestmentDialogProps {
	open: boolean;
	onClose: () => void;
	vehicleTarget: number;
}

export interface EditInvestmentDialogProps {
	open: boolean;
	onClose: () => void;
	investment: Investment;
}

export interface AddVehicleDialogProps {
	open: boolean;
	onClose: () => void;
}

export interface ConfirmDeleteDialogProps {
	open: boolean;
	onClose: () => void;
	onConfirm: () => void;
	deleteTarget: string;
	deleteTargetType: string;
}

export const AddInvestmentDialog = (props: AddInvestmentDialogProps) => {
	const formContext = useForm({
		shouldUnregister: true,
		resolver: yupResolver(investmentFormSchema),
	});

	const submit = useSubmit();
	const addAction = useFormAction('/add');
	const {enqueueSnackbar} = useSnackbar();

	const handleAdd = (data: FieldValues) => {
		const requestData: {[key: string]: string} = {};


		Object.entries(data)
			.map(([key, value]) => [key, value.toString()])
			.forEach(([key, value]) => (requestData[key] = value));

		//Check if we have known date fields, and convert them to yyyy-MM-dd
		convertDates(requestData);

		addInvestment(requestData)
			.then((investmentId) => {
				if (props.vehicleTarget > -1) {
					addInvestmentToVehicle(props.vehicleTarget, Number.parseInt(investmentId));
				} //Add investment to vehicle
				enqueueSnackbar('Investment added successfully.', {variant: 'success'});
				props.onClose();
				submit(null, {method: 'post', action: addAction});
			})
			.catch((error) => {
				enqueueSnackbar(`Failed to add investment: ${error.message}`, {variant: 'error'});
			});
	};

	return (
		<FormProvider {...formContext}>
			<Dialog open={props.open} maxWidth='md'>
				<DialogTitle>
					{props.vehicleTarget > -1 ? 'Add Investment to Vehicle' : 'Add Investment'}
				</DialogTitle>
				<Box sx={{padding: '2rem'}}>
					<InvestmentDataForm>
						<DialogActions>
							<Button onClick={props.onClose}>Cancel</Button>
							<Button onClick={formContext.handleSubmit(handleAdd)}>Add</Button>
						</DialogActions>
					</InvestmentDataForm>
				</Box>
			</Dialog>
		</FormProvider>
	);
};


export const EditInvestmentDialog = (props: EditInvestmentDialogProps) => {
	const formContext = useForm({
		shouldUnregister: true,
		resolver: yupResolver(investmentFormSchema),
	});
	console.log("EDITING:");
	console.log(props.investment);

	const submit = useSubmit();
	const addAction = useFormAction('/add');
	const {enqueueSnackbar} = useSnackbar();

	const handleAdd = (data: FieldValues) => {
		const requestData: {[key: string]: string} = {};


		Object.entries(data)
			.map(([key, value]) => [key, value.toString()])
			.forEach(([key, value]) => (requestData[key] = value));

		//Check if we have known date fields, and convert them to yyyy-MM-dd
		convertDates(requestData);

		//TODO: will need to make an edit investment method
		addInvestment(requestData)
			.then((investmentId) => {
				// if (props.vehicleTarget > -1) {
				// 	addInvestmentToVehicle(props.vehicleTarget, Number.parseInt(investmentId));
				// } //Add investment to vehicle
				enqueueSnackbar('Investment added successfully.', {variant: 'success'});
				props.onClose();
				submit(null, {method: 'post', action: addAction});
			})
			.catch((error) => {
				enqueueSnackbar(`Failed to add investment: ${error.message}`, {variant: 'error'});
			});
	};

	return (
		<FormProvider {...formContext}>
			<Dialog open={props.open} maxWidth='md'>
				<DialogTitle>
					{'Edit Investment'}
				</DialogTitle>
				<Box sx={{padding: '2rem'}}>
					<InvestmentDataForm selectedInvestment={props.investment}>
						<DialogActions>
							<Button onClick={props.onClose}>Cancel</Button>
							<Button onClick={formContext.handleSubmit(handleAdd)}>Save</Button>
						</DialogActions>
					</InvestmentDataForm>
				</Box>
			</Dialog>
		</FormProvider>
	);
};

export const AddVehicleDialog = (props: AddVehicleDialogProps) => {
	const formContext = useForm({
		shouldUnregister: true,
		resolver: yupResolver(vehicleFormSchema),
	});

	const submit = useSubmit();
	const addAction = useFormAction('/addVehicle');

	const {enqueueSnackbar} = useSnackbar();

	const handleVehicleAdd = (data: FieldValues) => {
		const requestData: {[key: string]: string} = {};

		Object.entries(data)
			.map(([key, value]) => [key, value.toString()])
			.forEach(([key, value]) => (requestData[key] = value));

		convertDates(requestData);

		addVehicle(requestData)
			.then(() => {
				enqueueSnackbar('Vehicle added successfully.', {variant: 'success'});
				props.onClose();
				submit(null, {method: 'post', action: addAction});
			})
			.catch((error) => {
				enqueueSnackbar(`Failed to add vehicle: ${error.message}`, {variant: 'error'});
			});
	};

	return (
		<FormProvider {...formContext}>
			<Dialog open={props.open} maxWidth='md'>
				<DialogTitle>Add Investment Vehicle</DialogTitle>
				<Box sx={{padding: '2rem'}}>
					<VehicleDataForm>
						<DialogActions>
							<Button onClick={props.onClose}>Cancel</Button>
							<Button onClick={formContext.handleSubmit(handleVehicleAdd)}>
								Add
							</Button>
						</DialogActions>
					</VehicleDataForm>
				</Box>
			</Dialog>
		</FormProvider>
	);
};

export const ConfirmDeleteDialog = (props: ConfirmDeleteDialogProps) => {
	const {enqueueSnackbar} = useSnackbar();

	const handleConfirm = () => {
		props.onConfirm();
		enqueueSnackbar(
			`${props.deleteTargetType === 'vehicle' ? 'Vehicle' : 'Invesetment'} was deleted`,
			{variant: 'success'},
		);
		props.onClose();
	};

	return (
		<Dialog open={props.open}>
			<DialogTitle>Confirm Deletion</DialogTitle>
			<DialogContent>
				Are you sure you want to delete{' '}
				{props.deleteTargetType + ' ' + props.deleteTarget + ' '}
				{props.deleteTargetType === 'vehicle' ? 'and all associated investments' : ''}?
			</DialogContent>
			<DialogActions>
				<Button onClick={props.onClose}>Cancel</Button>
				<Button color='error' onClick={handleConfirm}>
					Delete
				</Button>
			</DialogActions>
		</Dialog>
	);
};

interface AddExpenseDialogProps {
	show: boolean;
	onClose: () => void;
	investmentId: number;
	setNeedsUpdate: React.Dispatch<React.SetStateAction<boolean>>;
}

export const AddExpenseDialog = (props: AddExpenseDialogProps) => {
	const formContext = useForm({
		shouldUnregister: true,
	});
	const expenseType = useWatch({
		control: formContext.control,
		name: 'expenseType',
		defaultValue: 'OneTime',
	});

	const submitExpense = (data: any) => {
		const expenseData = {
			sourceInvestmentId: props.investmentId,
			...data,
		};

		addExpense(expenseData)
			.then(() => {
				enqueueSnackbar('Expense added', {variant: 'success'});
				props.setNeedsUpdate(true);
				props.onClose();
			})
			.catch((error) => {
				enqueueSnackbar(`Error adding expense: ${error.message}`, {variant: 'error'});
				console.log(error);
			});
	};

	const amountField = (
		<FormTextFieldCurrency
			control={formContext.control}
			name='amount'
			label='Expense Amount'
			errorField={undefined} 
			defaultValue={''}		/>
	);

	const expenseTypeField = (
		<FormSelectField
			control={formContext.control}
			name='expenseType'
			label='Expense Type'
			options={[
				{value: 'OneTime', label: 'One Time', tooltip: 'OneTime'},
				{value: 'Recurring', label: 'Recurring', tooltip: 'Recurring'},
			]}
			defaultOption='OneTime'
			errorField={undefined}
			disable={false}
		/>
	);

	const expenseDateField = (
		<FormDatePicker
			control={formContext.control}
			name='date'
			label='Expense Date'
			errorField={undefined}
			defaultValue={''}
		/>
	);

	const frequencyField = (
		<FormTextField
			control={formContext.control}
			name='frequency'
			label='Frequency (mos.)'
			errorField={undefined} 
			defaultValue={''}		/>
	);

	const startDateField = (
		<FormDatePicker
			control={formContext.control}
			name='startDate'
			label='Start Date'
			errorField={undefined}
			defaultValue={''}
		/>
	);

	const endDateField = (
		<FormDatePicker
			control={formContext.control}
			name='endDate'
			label='End Date'
			errorField={undefined}
			defaultValue={''}
		/>
	);

	return (
		<Dialog open={props.show} onClose={props.onClose}>
			<DialogTitle>Add Expense</DialogTitle>
			<DialogContent>
				<Grid container spacing={2} sx={{marginTop: '0.25rem'}}>
					<Grid item xs={4}>
						{amountField}
					</Grid>
					<Grid item xs={4}>
						{expenseTypeField}
					</Grid>
					<Grid item xs={4} />
					{expenseType === 'OneTime' ? (
						<Grid item xs={4}>
							{expenseDateField}
						</Grid>
					) : (
						<>
							<Grid item xs={4}>
								{frequencyField}
							</Grid>
							<Grid item xs={4}>
								{startDateField}
							</Grid>
							<Grid item xs={4}>
								{endDateField}
							</Grid>
						</>
					)}
				</Grid>
			</DialogContent>
			<DialogActions>
				<Button onClick={props.onClose}>Cancel</Button>
				<Button onClick={formContext.handleSubmit(submitExpense)}>Add</Button>
			</DialogActions>
		</Dialog>
	);
};
