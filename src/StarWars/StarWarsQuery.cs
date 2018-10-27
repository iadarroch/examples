using System;
using GraphQL.Types;
using StarWars.Types;

namespace StarWars
{
    public class StarWarsQuery : ObjectGraphType<object>
    {
        public StarWarsQuery(StarWarsData data)
        {
            Name = "Query";

            Field<CharacterInterface>("hero", resolve: context => data.GetDroidByIdAsync("3"));
            Field<HumansQuery>("humans", resolve: context => new { });
            Field<OkFluidHumansQuery>("okFluidHumans", resolve: context => new { });
            Field<BadFluidHumansQuery>("badFluidHumans", resolve: context => new { });
            Field<DroidsQuery>("droids", resolve: context => new { });
        }
    }

    public class HumansQuery : ObjectGraphType<object>
    {
        public HumansQuery(StarWarsData data)
        {
            Field<HumanType>(
                "human",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the human" }
                ),
                resolve: context => data.GetHumanByIdAsync(context.GetArgument<string>("id"))
            );

        }
    }

    public class OkFluidHumansQuery : ObjectGraphType<object>
    {
        public OkFluidHumansQuery(StarWarsData data)
        {
            Field<HumanType>()
                .Name("human")
                .Resolve(context => data.GetHumanByIdAsync("1")
            );
        }
    }
    public class BadFluidHumansQuery : ObjectGraphType<object>
    {
        public BadFluidHumansQuery(StarWarsData data)
        {
            Field<HumanType>()
                .Name("human")
                .Argument<NonNullGraphType<StringGraphType>>(Name = "thisIsTheArgId", Description = "id of the human")
                .Resolve(context => data.GetHumanByIdAsync(context.GetArgument<string>("id"))
                );
        }
    }

    public class DroidsQuery : ObjectGraphType<object>
    {
        public DroidsQuery(StarWarsData data)
        {
            Func<ResolveFieldContext, string, object> func = (context, id) => data.GetDroidByIdAsync(id);

            FieldDelegate<DroidType>(
                "droid",
                arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "id of the droid" }
                ),
                resolve: func
                );
        }
    }
}
