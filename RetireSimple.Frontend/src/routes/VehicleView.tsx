import {yupResolver} from '@hookform/resolvers/yup';
import {Box, Button, Divider, Typography} from '@mui/material';
import React from 'react';
import {FieldValues, FormProvider, useForm, useFormState} from 'react-hook-form';
import {useFormAction, useLoaderData, useSubmit} from 'react-router-dom';
import {VehicleFormDefaults, vehicleFormSchema} from '../forms/FormSchema';
import {FormVehicle} from '../Interfaces';
import {VehicleDataForm} from '../forms/VehicleDataForm';
import {updateVehicle} from '../api/VehicleApi';
import {VehicleModelGraph} from '../components/GraphComponents';
import {ConfirmDeleteDialog} from '../components/DialogComponents';

export const VehicleView = () => {
	const [showDelete, setShowDelete] = React.useState(false);
	const vehicleData = useLoaderData() as FormVehicle;
	const submit = useSubmit();
	const deleteAction = useFormAction('delete');
	const updateAction = useFormAction('update');
	const formContext = useForm({
		shouldUnregister: true,
		resolver: yupResolver(vehicleFormSchema),
		defaultValues: VehicleFormDefaults,
	});

	const {reset, control, handleSubmit} = formContext;
	const {isDirty, dirtyFields} = useFormState({control});

	//HACK React docs indicate this is problematic, should fix sometime
	React.useEffect(() => {
		formContext.reset(vehicleData, {keepErrors: true});
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [vehicleData]);

	const handleUpdate = handleSubmit((data: FieldValues) => {
		const requestData: {[key: string]: string} = {};
		Object.entries(dirtyFields).forEach(([key, value]) => {
			if (value === true) {
				requestData[key] = data[key].toString();
			}
		});

		updateVehicle(vehicleData.investmentVehicleId, requestData).then(() => {
			submit(null, {action: updateAction, method: 'post'});
		});
	});

	return (
		<Box sx={{display: 'flex', flexDirection: 'column'}}>
			<Box sx={{display: 'flex', flexDirection: 'column', justifyContent: 'flex-start'}}>
				<Typography variant='h6' component='div' sx={{flexGrow: 1, marginBottom: '1rem'}}>
					Vehicle Details: {vehicleData.investmentVehicleName}
				</Typography>
				<FormProvider {...formContext}>
					<VehicleDataForm defaultValues={vehicleData} disableTypeSelect={true}>
						<Divider sx={{paddingY: '5px'}} />
						<Box
							sx={{
								display: 'flex',
								flexDirection: 'row',
								justifyContent: 'flex-end',
							}}>
							<Button onClick={() => reset(vehicleData)}>Reset</Button>
							<Button color='error' onClick={() => setShowDelete(true)}>
								Delete
							</Button>
							<Button onClick={handleUpdate} disabled={!isDirty}>
								Update
							</Button>
						</Box>
					</VehicleDataForm>
				</FormProvider>
			</Box>
			<Box sx={{width: '100%', height: '100%'}}>
				<VehicleModelGraph vehicleId={vehicleData.investmentVehicleId} />
			</Box>
			<ConfirmDeleteDialog
				open={showDelete}
				onClose={() => setShowDelete(false)}
				onConfirm={() => submit(null, {action: deleteAction, method: 'delete'})}
				deleteTargetType='vehicle'
				deleteTarget={vehicleData.investmentVehicleName}
			/>
		</Box>
	);
};
