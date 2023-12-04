import { style  } from '@mui/system';
import {Box, Button, Divider, Tab, Tabs, Typography, Icon} from '@mui/material';

import {Investment} from '../Interfaces';
import { deleteInvestment } from '../api/InvestmentApi';
import { InvestmentModelGraph } from './GraphComponents';

export const InvestmentComponent = (investment: Investment, callback: Function) => {
//deleteInvestment

	return <body style={{backgroundColor: '#DCDCDC', margin: '15px'}}>
		<div style={{width: '900px', paddingLeft: '10px', paddingBottom: '10px', paddingRight: '0px'}}>
			<span>
				<h2> {investment.investmentName} 
					<Button onClick={() => callback()}>
						<Icon style={{color: 'black'}} baseClassName='material-icons'>edit_circle</Icon>
						<Typography variant='body1' component='div' sx={{marginLeft: '10px'}}>
							{/* Delete Investment */}
						</Typography>
					</Button>
					<Button onClick={() => deleteInvestment(investment.investmentId)}>
						<Icon style={{color: 'black'}} baseClassName='material-icons'>delete_circle</Icon>
						<Typography variant='body1' component='div' sx={{marginLeft: '10px'}}>
							{/* Delete Investment */}
						</Typography>
					</Button>
				</h2>				
			</span>
			<span style={{display: 'flex'}}>
				<div style={{flex: '50%'}}>
					<div>Ticker: {investment.investmentData.stockTicker}</div>
					<div>Quantity: {investment.investmentData.stockQuantity}</div>	
				</div>	
				<div style={{flex: '50%', width: '50px'}}>
					<InvestmentModelGraph investmentId={investment.investmentId} />
				</div>
			</span>
		</div>
	</body>
}