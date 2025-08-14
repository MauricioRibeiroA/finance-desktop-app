from flask import Flask, render_template, request, jsonify
from flask_sqlalchemy import SQLAlchemy
from datetime import datetime, date
import os

app = Flask(__name__)

# Database configuration
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///finance.db'
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False

db = SQLAlchemy(app)

# Models
class Transaction(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    description = db.Column(db.String(200), nullable=False)
    amount = db.Column(db.Float, nullable=False)
    category = db.Column(db.String(50), nullable=False)
    transaction_type = db.Column(db.String(20), nullable=False)  # 'income' or 'expense'
    date = db.Column(db.Date, nullable=False, default=date.today)
    created_at = db.Column(db.DateTime, default=datetime.utcnow)

    def to_dict(self):
        return {
            'id': self.id,
            'description': self.description,
            'amount': self.amount,
            'category': self.category,
            'transaction_type': self.transaction_type,
            'date': self.date.isoformat(),
            'created_at': self.created_at.isoformat()
        }

class Category(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String(50), nullable=False, unique=True)
    icon = db.Column(db.String(50), default='💰')
    color = db.Column(db.String(10), default='#007bff')

    def to_dict(self):
        return {
            'id': self.id,
            'name': self.name,
            'icon': self.icon,
            'color': self.color
        }

# Create tables
def create_tables():
    if not os.path.exists('database'):
        os.makedirs('database')
    
    with app.app_context():
        db.create_all()
        
        # Add default categories if none exist
        if Category.query.count() == 0:
            default_categories = [
                {'name': 'Alimentação', 'icon': '🍽️', 'color': '#ff6b6b'},
                {'name': 'Transporte', 'icon': '🚗', 'color': '#4ecdc4'},
                {'name': 'Saúde', 'icon': '🏥', 'color': '#45b7d1'},
                {'name': 'Educação', 'icon': '📚', 'color': '#f39c12'},
                {'name': 'Lazer', 'icon': '🎮', 'color': '#9b59b6'},
                {'name': 'Casa', 'icon': '🏠', 'color': '#1abc9c'},
                {'name': 'Salário', 'icon': '💼', 'color': '#27ae60'},
                {'name': 'Freelance', 'icon': '💻', 'color': '#3498db'},
                {'name': 'Investimentos', 'icon': '📈', 'color': '#e74c3c'},
                {'name': 'Outros', 'icon': '📦', 'color': '#95a5a6'}
            ]
            
            for cat_data in default_categories:
                category = Category(**cat_data)
                db.session.add(category)
            
            db.session.commit()

# Routes
@app.route('/')
def index():
    return render_template('index.html')

@app.route('/api/transactions', methods=['GET'])
def get_transactions():
    page = request.args.get('page', 1, type=int)
    per_page = request.args.get('per_page', 10, type=int)
    
    transactions = Transaction.query.order_by(Transaction.date.desc()).paginate(
        page=page, per_page=per_page, error_out=False)
    
    return jsonify({
        'transactions': [t.to_dict() for t in transactions.items],
        'total': transactions.total,
        'pages': transactions.pages,
        'current_page': page
    })

@app.route('/api/transactions', methods=['POST'])
def add_transaction():
    data = request.json
    
    try:
        transaction = Transaction(
            description=data['description'],
            amount=float(data['amount']),
            category=data['category'],
            transaction_type=data['transaction_type'],
            date=datetime.strptime(data['date'], '%Y-%m-%d').date()
        )
        
        db.session.add(transaction)
        db.session.commit()
        
        return jsonify({'success': True, 'transaction': transaction.to_dict()}), 201
    
    except Exception as e:
        return jsonify({'success': False, 'error': str(e)}), 400

@app.route('/api/transactions/<int:transaction_id>', methods=['DELETE'])
def delete_transaction(transaction_id):
    transaction = Transaction.query.get_or_404(transaction_id)
    db.session.delete(transaction)
    db.session.commit()
    return jsonify({'success': True})

@app.route('/api/categories', methods=['GET'])
def get_categories():
    categories = Category.query.all()
    return jsonify([cat.to_dict() for cat in categories])

@app.route('/api/dashboard', methods=['GET'])
def get_dashboard():
    # Get current month data
    today = date.today()
    current_month = today.replace(day=1)
    
    transactions = Transaction.query.filter(Transaction.date >= current_month).all()
    
    total_income = sum(t.amount for t in transactions if t.transaction_type == 'income')
    total_expense = sum(t.amount for t in transactions if t.transaction_type == 'expense')
    balance = total_income - total_expense
    
    # Category breakdown for expenses
    expense_by_category = {}
    for t in transactions:
        if t.transaction_type == 'expense':
            expense_by_category[t.category] = expense_by_category.get(t.category, 0) + t.amount
    
    return jsonify({
        'total_income': total_income,
        'total_expense': total_expense,
        'balance': balance,
        'expense_by_category': expense_by_category,
        'transaction_count': len(transactions)
    })

if __name__ == '__main__':
    create_tables()
    print("🚀 Finance Web App iniciando...")
    print("📱 Acesse: http://localhost:5000")
    app.run(debug=True, host='0.0.0.0', port=5000)
