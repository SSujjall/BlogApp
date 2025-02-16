import Layout from "../components/layout/Layout";

const NotFound = () => {
  return (
    <Layout>
      <div className="flex flex-col justify-center items-center h-full -mt-5">
        <span className="bg-red-500 text-7xl p-4 rounded-2xl inline-block text-white">
          404
        </span>
        <p className="text-7xl text-center">Not Found</p>
      </div>
    </Layout>
  );
};

export default NotFound;
