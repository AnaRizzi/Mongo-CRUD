using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace Crud_Mongo
{
    public class MongoConnection
    {
        private readonly string _stringConnection;
        private readonly IMongoClient client;
        private readonly IMongoDatabase bancoDados;
        private readonly IMongoCollection<Books> colecao;

        public MongoConnection()
        {
            _stringConnection = "mongodb://localhost:27017";
            client = new MongoClient(_stringConnection);

            //vai buscar o banco de dados, se não existir, ele cria
            bancoDados = client.GetDatabase("Books");

            //acessar coleção, se não existir cria:
            colecao = bancoDados.GetCollection<Books>("Books");

        }

        public async Task InsertOne(Books book)
        {
            //incluir documento:
            await colecao.InsertOneAsync(book);

        }

        public async Task InsertMany(IEnumerable<Books> books)
        {
            //para incluir uma lista, inserir vários documentos de uma vez:
            await colecao.InsertManyAsync(books);

        }

        public async Task<List<Books>> GetAll()
        {
            //buscar documentos, sem nenhum critério de busca:
            var listaLivros = await colecao.Find(new BsonDocument()).ToListAsync();

            return listaLivros;
        }

        public async Task<List<Books>> GetByAuthor(string author)
        {
            //criar uma regex para a expressão que será buscada (IgnoreCase para caseInsensitive, ou None)
            var queryExpr = new BsonRegularExpression(new Regex(author, RegexOptions.IgnoreCase));

            //cria o filtro
            var filtro = Builders<Books>.Filter;

            //cria a condição passando o regex, irá buscar aquele trecho em qualquer lugar
            var condicao = filtro.Regex(x => x.Author, queryExpr);


            //procurar campos que comecem com o termo da busca (case-sensitive)
            var condicao2 = filtro.Regex(x => x.Author, "^" + author + ".*");

            //procurar campos que terminem com o termo da busca (case-sensitive)
            var condicao3 = filtro.Regex(x => x.Author, author + "$");


            //buscar documentos, com critério de busca, passando a condição desejada:
            var listaLivros = await colecao.Find(condicao).ToListAsync();

            return listaLivros;
        }

        public async Task<Books> GetById(string id)
        {
            //cria o filtro que você precisa
            var filtro = Builders<Books>.Filter;

            //cria a condição (igual (eq), maior (gte = >=), etc....)
            var condicao = filtro.Eq(x => x.Id, id);

            //buscar documentos, com critério de busca, passando a condição desejada:
            var livro = await colecao.Find(condicao).FirstOrDefaultAsync();

            return livro;
        }

        public async Task<List<Books>> GetAlfabeticalOrder()
        {
            //buscar documentos, com critério de busca:
            var listaLivros = await colecao.Find(new BsonDocument()).SortBy(x => x.Title).ToListAsync();

            return listaLivros;
        }

        public async Task<Books> Update(string id, Books newBook)
        {
            //cria o filtro que você precisa
            var filtro = Builders<Books>.Filter;

            //cria a condição (igual (eq), maior (gte = >=), etc....)
            var condicao = filtro.Eq(x => x.Id, id);

            //buscar o documento:
            var livro = await colecao.Find(condicao).FirstOrDefaultAsync();

            //alterar o campo a ser atualizado:
            livro = newBook;

            //atualizar no banco, passando a condição de busca do que sera alterado e a informação nova
            await colecao.ReplaceOneAsync(condicao, livro);

            return livro;
        }

        public async Task Delete(string id)
        {
            //cria o filtro que você precisa
            var filtro = Builders<Books>.Filter;
            //cria a condição pra encontrar o livro
            var condicao = filtro.Eq(x => x.Id, id);

            //excluir o documento, passa a condição de busca:
            var livro = await colecao.DeleteOneAsync(condicao);
            //ou DeleteManyAsync, se for para apagar vários de uma vez
        }



    }
}
