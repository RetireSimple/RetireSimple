import {yupResolver} from '@hookform/resolvers/yup';
import {Box, Button, Dialog, DialogActions, DialogTitle} from '@mui/material';
import {FieldValues, FormProvider, useForm} from 'react-hook-form';
import {useFormAction, useSubmit} from 'react-router-dom';
import {addVehicle} from '../../api/VehicleApi';
import {vehicleFormSchema} from '../../data/FormSchema';
import {VehicleDataForm} from '../../forms/VehicleDataForm';

export interface AddVehicleDialogProps {
	open: boolean;
	onClose: () => void;
}

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
