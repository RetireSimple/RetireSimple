import {yupResolver} from '@hookform/resolvers/yup';
import {Box, Button, Dialog, DialogActions, DialogTitle} from '@mui/material';
import {FieldValues, FormProvider, useForm} from 'react-hook-form';
import {useFormAction, useSubmit} from 'react-router-dom';
import {addInvestment} from '../api/InvestmentApi';
import {addInvestmentToVehicle, addVehicle} from '../api/VehicleApi';
import {investmentFormSchema, vehicleFormSchema} from '../forms/FormSchema';
import {InvestmentDataForm} from '../forms/InvestmentDataForm';
import {VehicleDataForm} from '../forms/VehicleDataForm';

export interface AddInvestmentDialogProps {
	open: boolean;
	onClose: () => void;
	vehicleTarget: number;
}

export interface AddVehicleDialogProps {
	open: boolean;
	onClose: () => void;
}

export const AddInvestmentDialog = (props: AddInvestmentDialogProps) => {
	const formContext = useForm({
		shouldUnregister: true,
		resolver: yupResolver(investmentFormSchema),
	});

	const submit = useSubmit();
	const addAction = useFormAction('/add');

	const handleAdd = (data: FieldValues) => {
		const requestData: {[key: string]: string} = {};

		Object.entries(data)
			.map(([key, value]) => [key, value.toString()])
			.forEach(([key, value]) => (requestData[key] = value));

		addInvestment(requestData).then((investmentId) => {
			if (props.vehicleTarget > -1) {
				addInvestmentToVehicle(props.vehicleTarget, Number.parseInt(investmentId));
			} //Add investment to vehicle
			props.onClose();
			submit(null, {method: 'post', action: addAction});
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

export const AddVehicleDialog = (props: AddVehicleDialogProps) => {
	const formContext = useForm({
		shouldUnregister: true,
		resolver: yupResolver(vehicleFormSchema),
	});

	const submit = useSubmit();
	const addAction = useFormAction('/addVehicle');

	const handleVehicleAdd = (data: FieldValues) => {
		const requestData: {[key: string]: string} = {};

		Object.entries(data)
			.map(([key, value]) => [key, value.toString()])
			.forEach(([key, value]) => (requestData[key] = value));

		addVehicle(requestData).then(() => {
			props.onClose();
			submit(null, {method: 'post', action: addAction});
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
